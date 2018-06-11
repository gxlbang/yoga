/*
* 姓名:gxlbang
* 类名:Am_Bill
* CLR版本：
* 创建时间:2018-04-17 18:56:53
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
    /// Am_Bill控制器
    /// </summary>
    public class Am_BillController : PublicController<Am_Bill>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords,
            [DefaultValue(-1)]int Status, string ProvinceId, string CityId, string CountyId, string BeginTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_BillBll bll = new Am_BillBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Status, ProvinceId, CityId, CountyId, BeginTime, EndTime);
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
        /// 获得账单详情
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetBillContent(string KeyValue)
        {
            Am_BillBll bll = new Am_BillBll();
            return Json(bll.GetPageList(KeyValue));
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        public void ExportExcel(string keywords,
            [DefaultValue(-1)]int Status, string ProvinceId, string CityId, string CountyId, string BeginTime, string EndTime)
        {
            Am_BillBll bll = new Am_BillBll();
            var ListData = bll.GetPageList(keywords, Status, ProvinceId, CityId, CountyId, BeginTime, EndTime);
            var newlist = new List<Am_BillNew>();
            foreach (var item in ListData)
            {
                var model = new Am_BillNew();
                model.Address = item.Address;
                model.AmmeterCode = item.AmmeterCode;
                model.BillCode = item.BillCode;
                model.Cell = item.Cell;
                model.City = item.City;
                model.County = item.County;
                model.Floor = item.Floor;
                model.Money = item.Money.Value.ToString("0.00");
                model.Province = item.Province;
                model.Room = item.Room;
                model.StatusStr = item.StatusStr;
                model.F_U_Name = item.F_U_Name;
                model.OtherFees = item.OtherFees.Value.ToString("0.00");
                model.PayTime = item.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.SendTime = item.SendTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.T_U_Name = item.T_U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "账单号:BillCode", "电表号:AmmeterCode", "运营商:F_U_Name",
                "租户:T_U_Name","状态:StatusStr","账单金额:Money","滞纳金:OtherFees","账单日期:SendTime","支付日期:PayTime",
                "省:Province","市:City", "区:County","单元:Cell","楼层:Floor","房号:Room", "地址:Address" };
            DeriveExcel.ListToExcel<Am_BillNew>(newlist, columns, "账单数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}