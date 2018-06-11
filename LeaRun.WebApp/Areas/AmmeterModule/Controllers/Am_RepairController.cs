/*
* 姓名:gxlbang
* 类名:Am_Repair
* CLR版本：
* 创建时间:2018-04-17 19:07:30
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
    /// Am_Repair控制器
    /// </summary>
    public class Am_RepairController : PublicController<Am_Repair>
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
                Am_RepairBll bll = new Am_RepairBll();
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
        /// 获取报修图片
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetAm_RepairImage(string KeyValue)
        {
            Am_RepairBll bll = new Am_RepairBll();
            return Json(bll.GetImagePageList(KeyValue));
        }
        /// <summary>
        /// 获得报修反馈
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetAm_RepairAnswer(string KeyValue)
        {
            Am_RepairBll bll = new Am_RepairBll();
            return Json(bll.GetPageModel(KeyValue));
        }
        /// <summary>
        /// 数据导出
        /// </summary>
        public void ExportExcel(string keywords,
            [DefaultValue(-1)]int Status, string ProvinceId, string CityId, string CountyId, string BeginTime, string EndTime)
        {
            Am_RepairBll bll = new Am_RepairBll();
            var ListData = bll.GetPageList(keywords, Status, ProvinceId, CityId, CountyId, BeginTime, EndTime);
            var newlist = new List<Am_RepairNew>();
            foreach (var item in ListData)
            {
                var model = new Am_RepairNew();
                model.Address = item.Address;
                model.AmmeterCode = item.AmmeterCode;
                model.Cell = item.Cell;
                model.City = item.City;
                model.County = item.County;
                model.Floor = item.Floor;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Province = item.Province;
                model.Room = item.Room;
                model.StatusStr = item.StatusStr;
                model.F_Name = item.F_Name;
                model.F_UserName = item.F_UserName;
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;
                model.RepairCode = item.RepairCode.Value.ToString();

                newlist.Add(model);
            }
            string[] columns = new string[] { "报修编号:RepairCode","电表编号:AmmeterCode", "租户:UserName", "租户姓名:U_Name",
                "业主:F_UserName","业主姓名:F_Name","状态:StatusStr","报修时间:CreateTime",
                "省:Province","市:City", "区:County", "地址:Address","单元:Cell","楼层:Floor","房号:Room" };
            DeriveExcel.ListToExcel<Am_RepairNew>(newlist, columns, "报修数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}