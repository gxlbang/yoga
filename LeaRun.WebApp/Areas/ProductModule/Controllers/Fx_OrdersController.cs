/*
 * 姓名:gxlbang
 * 类名:IISWorker
 * CLR版本：4.0.30319.42000
 * 创建时间:2017/11/1 16:52:09
 * 功能描述:
 * 
 * 修改历史：
 * 
 * ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
 * ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
 * ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
 */
using LeaRun.Business;
using LeaRun.Entity;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.ProductModule.Controllers
{
    /// <summary>
    /// Fx_Orders控制器
    /// </summary>
    public class Fx_OrdersController : PublicController<Fx_Orders>
    {
        Fx_OrdersBll bll = new Fx_OrdersBll();

        /// <summary>
        /// 返回产品栏目列表JSON
        /// </summary>
        /// <param name="jqgridparam">表格参数</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string StartTime, string EndTime,
            string Keyword, string Stuts, [DefaultValue(0)]int IsAll)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = bll.GetPageList(ref jqgridparam);
                var ListData = bll.GetPageList1(ref jqgridparam, StartTime, EndTime, Keyword, Stuts, IsAll);
                var newlist = new List<Fx_Orders>();
                foreach (var item in ListData)
                {
                    item.Arddress = item.Province + item.City + item.County + item.Arddress;

                    newlist.Add(item);
                }
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = newlist
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
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public virtual ActionResult SubmitOrderForm(Fx_Orders entity, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    Fx_Orders Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    //输入了快递单号即发货,且订单状态在未发货状态
                    if (!string.IsNullOrEmpty(entity.Ex_NO) && entity.Stuts == 2)
                    {
                        entity.Ex_Time = DateTime.Now;
                        entity.Ex_UserId = ManageProvider.Provider.Current().UserId;
                    }
                    entity.Resutl = GetOrderResult(entity.Stuts??0);
                    IsOk = repositoryfactory.Repository().Update(entity);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    entity.Create();
                    IsOk = repositoryfactory.Repository().Insert(entity);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 更改订单状态
        /// </summary>
        /// <param name="Status">状态码</param>
        /// <returns></returns>
        public ActionResult SetOrders(string KeyValue, int Status)
        {
            Fx_Orders entity = repositoryfactory.Repository().FindEntity(KeyValue);
            entity.Modify(KeyValue);
            entity.Stuts = Status;
            entity.Resutl = GetOrderResult(Status);
            int IsOk = repositoryfactory.Repository().Update(entity);
            return Content(new JsonMessage { Success = IsOk > 0, Code = IsOk.ToString(), Message = "操作" + (IsOk > 0 ? "成功" : "失败") }.ToString());
        }

        private string GetOrderResult(int status)
        {
            switch (status)
            {
                case 1:
                    return "未发货";
                case 2:
                    return "有效订单";
                case 3:
                    return "已发货";
                case 4:
                    return "已完成";
                case 5:
                    return "已退货";
                case 6:
                    return "可疑";
                case 7:
                    return "无效";
                default:
                    return "未知";
            }
        }
    }
}