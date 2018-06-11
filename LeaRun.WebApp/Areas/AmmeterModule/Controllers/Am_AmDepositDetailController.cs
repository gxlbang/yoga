/*
* 姓名:gxlbang
* 类名:Am_AmDepositDetail
* CLR版本：
* 创建时间:2018-04-17 19:10:15
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
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_AmDepositDetail控制器
    /// </summary>
    public class Am_AmDepositDetailController : PublicController<Am_AmDepositDetail>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmDepositDetailBll bll = new Am_AmDepositDetailBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords);
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
        public void ExportExcel(string keywords)
        {
            Am_AmDepositDetailBll bll = new Am_AmDepositDetailBll();
            var ListData = bll.GetPageList(keywords);
            var newlist = new List<Am_AmDepositDetailNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmDepositDetailNew();
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.CurrMoney = item.CurrMoney.Value.ToString("0.00");
                model.Mark = item.Mark;
                model.Money = item.Money.Value.ToString("0.00");
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "运营商:U_Name","手机号:UserName","返还金额:Money",
                "剩余金额:CurrMoney", "返还时间:CreateTime","备注信息:Mark" };
            DeriveExcel.ListToExcel<Am_AmDepositDetailNew>(newlist, columns, "押金明细数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}