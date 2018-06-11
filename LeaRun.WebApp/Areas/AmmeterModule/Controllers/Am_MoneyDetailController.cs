/*
* 姓名:gxlbang
* 类名:Am_MoneyDetail
* CLR版本：
* 创建时间:2018-04-15 14:49:31
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
    /// Am_MoneyDetail控制器
    /// </summary>
    public class Am_MoneyDetailController : PublicController<Am_MoneyDetail>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords,
             string BeginTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_MoneyDetailBll bll = new Am_MoneyDetailBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, BeginTime, EndTime);
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
        public void ExportExcel(string keywords,string BeginTime, string EndTime)
        {
            Am_MoneyDetailBll bll = new Am_MoneyDetailBll();
            var ListData = bll.GetPageList(keywords, BeginTime, EndTime);
            var newlist = new List<Am_MoneyDetailNew>();
            foreach (var item in ListData)
            {
                var model = new Am_MoneyDetailNew();
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.CurrMoney = item.CurrMoney.Value.ToString("0.00");
                model.Money = item.Money.Value.ToString("0.00");
                model.OperateTypeStr = item.OperateTypeStr;
                model.Remark = item.Remark;
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "用户:UserName", "姓名:U_Name",
                "操作类型:OperateTypeStr","变动金额:Money","变动时间:CreateTime","变动后金额:CurrMoney",
                "备注:Remark" };
            DeriveExcel.ListToExcel<Am_MoneyDetailNew>(newlist, columns, "充值数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}