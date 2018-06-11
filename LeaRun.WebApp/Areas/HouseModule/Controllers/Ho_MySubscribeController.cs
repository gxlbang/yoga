/*
* 姓名:gxlbang
* 类名:Ho_MySubscribe
* CLR版本：
* 创建时间:2017-12-05 11:54:11
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.HouseModule.Controllers
{
    /// <summary>
    /// 我的预约控制器
    /// </summary>
    public class Ho_MySubscribeController : PublicController<Ho_MySubscribe>
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        //[ManagerPermission(PermissionMode.Enforce)]
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("3015aec6-3b63-4dff-9c4b-a3c4a67e2148");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }
        
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string StartTime, string EndTime,
            string Keyword, string Stuts)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_MySubscribeBll bll = new Ho_MySubscribeBll();
                var ListData = bll.GetPageList(ref jqgridparam, StartTime, EndTime, Keyword, Stuts);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="pclass">栏目信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue, Ho_MySubscribe model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var oldModel = database.FindEntity<Ho_MySubscribe>(KeyValue);
                if (oldModel == null)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "预约不存在" }.ToString());
                }
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                //如果当前状态为已预约,则处理为接待中
                if (model.Status == 0)
                {
                    model.Status = 1;
                    model.StatusStr = GetStatusStr(model.Status ?? 0);
                }
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    model.ReTime = DateTime.Now;
                    model.ReUserNumber = ManageProvider.Provider.Current().UserId;
                    model.ReUser = ManageProvider.Provider.Current().Account;
                    model.Modify(KeyValue);
                    var IsOk = database.Update(model, isOpenTrans);
                    //如果管理员点了预约结束或者完成,则把业务信息状态更新为暂无预约
                    //暂不在此更新
                    if (IsOk > 0 && model.Status == 9)
                    {
                        //更新我的业务-后台只有点完成的时候才释放用户业务为暂无预约
                        //对于我的业务Ho_MyHouseInfo只有两个状态,即已预约和暂无预约
                        var mhmodel = database.FindEntity<Ho_MyHouseInfo>(oldModel.MHNumber);
                        mhmodel.Status = 0;
                        mhmodel.StatusStr = "暂无预约";
                        mhmodel.Modify(mhmodel.Number);
                        database.Update(mhmodel, isOpenTrans);
                    }
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "预约接待安排");
                }
                else //新建
                {
                    model.Create();
                    database.Insert(model, isOpenTrans);
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 更新预约状态
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ActionResult SetOrders(string KeyValue, int Status)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                Ho_MySubscribe entity = database.FindEntity<Ho_MySubscribe>(KeyValue);
                entity.Modify(KeyValue);
                entity.Status = Status;
                entity.StatusStr = GetStatusStr(Status);
                int IsOk = database.Update(entity, isOpenTrans);
                //关闭和完成都释放
                if (IsOk > 0 && (Status == 9||Status==2))
                {
                    //更新我的业务-后台点关闭和完成的时候才释放用户业务为暂无预约
                    //对于我的业务Ho_MyHouseInfo只有两个状态,即已预约和暂无预约
                    var mhmodel = database.FindEntity<Ho_MyHouseInfo>(entity.MHNumber);
                    mhmodel.Status = 0;
                    mhmodel.StatusStr = "暂无预约";
                    mhmodel.Modify(mhmodel.Number);
                    database.Update(mhmodel, isOpenTrans);
                }
                database.Commit();
                return Content(new JsonMessage { Success = IsOk > 0, Code = IsOk.ToString(), Message = "操作" + (IsOk > 0 ? "成功" : "失败") }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //获取状态字符
        public string GetStatusStr(int s)
        {
            switch (s)
            {
                case -1:
                    return "取消预约";
                case 0:
                    return "已提交";
                case 1:
                    return "接待中";
                case 2:
                    return "关闭预约";
                case 9:
                    return "已完成";
                default:
                    return "未知";
            }
        }
    }
}