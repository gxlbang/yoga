/*
* 姓名:gxlbang
* 类名:Am_AmDeposit
* CLR版本：
* 创建时间:2018-04-17 19:10:01
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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_AmDeposit控制器
    /// </summary>
    public class Am_AmDepositController : PublicController<Am_AmDeposit>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords, [DefaultValue(-1)]int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmDepositBll bll = new Am_AmDepositBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Stuts, ProvinceId, CityId, CountyId);
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
        public void ExportExcel([DefaultValue(-1)]int Stuts, string keywords, string ProvinceId, string CityId, string CountyId)
        {
            Am_AmDepositBll bll = new Am_AmDepositBll();
            var ListData = bll.GetPageList(keywords, Stuts, ProvinceId, CityId, CountyId);
            var newlist = new List<Am_AmDepositNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmDepositNew();
                model.Address = item.Address;
                model.Ammeter_Code = item.Ammeter_Code;
                model.Cell = item.Cell;
                model.City = item.City;
                model.County = item.County;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.CurrMoeny = item.CurrMoeny.ToString();
                model.Floor = item.Floor;
                model.Money = item.Money.Value.ToString("0.00");
                model.Province = item.Province;
                model.Room = item.Room;
                model.StatusStr = item.StatusStr;
                model.U_Name = item.U_Name;
                model.UpdateTime = item.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");

                newlist.Add(model);
            }
            string[] columns = new string[] { "运营商:U_Name","电表号:Ammeter_Code","省:Province",
                "市:City", "区:County","地址:Address","单元:Cell","楼层:Floor","房号:Room", 
                "押金时间:CreateTime", "状态:StatusStr","押金金额:Money", "剩余金额:CurrMoeny",
                "最后更新时间:UpdateTime" };
            DeriveExcel.ListToExcel<Am_AmDepositNew>(newlist, columns, "押金数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}