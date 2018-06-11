/*
* 姓名:gxlbang
* 类名:Am_Charge
* CLR版本：
* 创建时间:2018-04-17 10:13:57
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

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_Charge控制器
    /// </summary>
    public class Am_ChargeController : PublicController<Am_Charge>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords,
            int Status, int ChargeType, string BeginTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_ChargeBll bll = new Am_ChargeBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Status, ChargeType, BeginTime, EndTime);
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
        /// 数据导出
        /// </summary>
        public void ExportExcel(string keywords,
            [DefaultValue(-1)]int Status, int ChargeType, string BeginTime, string EndTime)
        {
            Am_ChargeBll bll = new Am_ChargeBll();
            var ListData = bll.GetPageList(keywords, Status, ChargeType, BeginTime, EndTime);
            var newlist = new List<Am_ChargeNew>();
            foreach (var item in ListData)
            {
                var model = new Am_ChargeNew();
                model.AmmeterCode = item.AmmeterCode;
                model.ChargeTypeStr = item.ChargeTypeStr;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Money = item.Money.Value.ToString("0.00");
                model.OrderNumber = item.OrderNumber;
                model.OutNumber = item.OutNumber;
                model.PayType = item.PayType;
                model.StatusStr = item.StatusStr;
                model.SucTime = item.SucTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;
                newlist.Add(model);
            }
            string[] columns = new string[] { "编号:OrderNumber", "外部编号:OutNumber", "电表号:AmmeterCode",
                "用户:UserName","姓名:U_Name","充值类型:ChargeTypeStr","支付方式:PayType","充值金额:Moeny","状态:StatusStr",
                "充值时间:CreateTime","成功时间:SucTime" };
            DeriveExcel.ListToExcel<Am_ChargeNew>(newlist, columns, "充值数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}