/*
* 姓名:gxlbang
* 类名:Am_AmmeterType
* CLR版本：
* 创建时间:2018-04-11 17:19:30
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
using Extensions;
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

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_AmmeterType控制器
    /// </summary>
    public class Am_AmmeterTypeController : PublicController<Am_AmmeterType>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string Keyword)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmmeterTypeBll bll = new Am_AmmeterTypeBll();
                var ListData = bll.GetPageList(ref jqgridparam, Keyword);
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
        public ActionResult Edit()
        {
            return View();
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="pclass">栏目信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue, Am_AmmeterType model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    model.Modify(KeyValue);
                    var IsOk = database.Update(model, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "电表类型" + Message);
                }
                else //新建
                {
                    model.UserName = ManageProvider.Provider.Current().Account;
                    model.UserRealName = ManageProvider.Provider.Current().UserName;
                    model.UserNumber = ManageProvider.Provider.Current().UserId;
                    model.Create();
                    var IsOk = database.Insert(model, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "电表类型" + Message);
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
        //获取字典信息
        public ActionResult GetAmmeterType(string Dic_Code)
        {
            List<Fx_Dictionary> list;
            var dicl = HttpContext.Cache["DicList"];
            if (dicl != null)
            {
                list = dicl as List<Fx_Dictionary>;
            }
            else
            {
                IDatabase database = DataFactory.Database();
                list = database.FindList<Fx_Dictionary>();
                HttpContext.Cache["DicList"] = list;
            }
            return Content(list.Where(o => o.Dic_Code == Dic_Code).ToJson());
        }
    }
}