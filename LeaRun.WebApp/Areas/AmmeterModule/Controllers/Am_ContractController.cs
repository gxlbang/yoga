/*
* 姓名:gxlbang
* 类名:Am_Contract
* CLR版本：
* 创建时间:2018-05-02 16:51:11
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
    /// Am_Contract控制器
    /// </summary>
    public class Am_ContractController : PublicController<Am_Contract>
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
                Am_ContractBll bll = new Am_ContractBll();
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
            Am_ContractBll bll = new Am_ContractBll();
            return Json(bll.GetPageList(KeyValue));
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        public void ExportExcel(string keywords,
            [DefaultValue(-1)]int Status, string ProvinceId, string CityId, string CountyId, string BeginTime, string EndTime)
        {
            Am_ContractBll bll = new Am_ContractBll();
            var ListData = bll.GetPageList(keywords, Status, ProvinceId, CityId, CountyId, BeginTime, EndTime);
            var newlist = new List<Am_ContractNew>();
            foreach (var item in ListData)
            {
                var model = new Am_ContractNew();
                model.Address = item.Address;
                model.AmmeterCode = item.AmmeterCode;
                model.Cell = item.Cell;
                model.City = item.City;
                model.County = item.County;
                model.Floor = item.Floor;
                model.CreateAddress = item.CreateAddress;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Province = item.Province;
                model.Room = item.Room;
                model.StatusStr = item.StatusStr;
                model.F_U_Name = item.F_U_Name;
                model.F_UserName = item.F_UserName;
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "电表表号:AmmeterCode", "租户:UserName", "租户姓名:U_Name",
                "签订日期:CreateTime","签订地址:CreateAddress","状态:StatusStr","运营商:F_UserName","运营商姓名:F_U_Name",
                "省:Province","市:City", "区:County","单元:Cell","楼层:Floor","房号:Room", "地址:Address" };
            DeriveExcel.ListToExcel<Am_ContractNew>(newlist, columns, "合同数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}