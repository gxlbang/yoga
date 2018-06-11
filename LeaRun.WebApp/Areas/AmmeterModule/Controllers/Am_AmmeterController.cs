/*
* 姓名:gxlbang
* 类名:Am_Ammeter
* CLR版本：
* 创建时间:2018-04-14 10:54:05
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
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_Ammeter控制器
    /// </summary>
    public class Am_AmmeterController : PublicController<Am_Ammeter>
    {
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("235ddccb-9114-404b-9632-06715c6ad496");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }
        /// <summary>
        /// 删除电表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult DeleteAmmeter(string KeyValue)
        {
            var Message = "删除失败。";
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var model = database.FindEntity<Am_Ammeter>(KeyValue);
                if (model == null && string.IsNullOrEmpty(model.Number))
                {
                    Message = "数据异常";
                    WriteLog(-1, KeyValue, Message);
                    return Content(new JsonMessage { Success = false, Code = "-1", Message= Message }.ToString());
                }
                model.Status = 9;
                model.StatusStr = "已删除";
                model.UpdateTime = DateTime.Now;
                model.Modify(model.Number);
                var isok = database.Update(model, isOpenTrans);
                if (isok < 1)
                {
                    isOpenTrans.Rollback();
                    WriteLog(-1, KeyValue, Message);
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
                }
                //更细采集器下属电表数量
                var cmodel = database.FindEntity<Am_Collector>(model.Collector_Number);
                if (cmodel != null && !string.IsNullOrEmpty(cmodel.Number))
                {
                    cmodel.AmCount = (cmodel.AmCount != null && cmodel.AmCount > 0) ? cmodel.AmCount - 1 : 0;
                    if (database.Update(cmodel, isOpenTrans) < 1)
                    {
                        isOpenTrans.Rollback();
                        WriteLog(-1, KeyValue, Message);
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
                    }
                }
                //更新关系表
                StringBuilder sql = new StringBuilder();
                sql.Append("update Am_AmmeterPermission set Status = 9,StatusStr='已删除' where Ammeter_Number = '" + model.Number + "'");
                database.ExecuteBySql(sql, isOpenTrans);
                isOpenTrans.Commit();
                WriteLog(1, KeyValue, Message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                isOpenTrans.Rollback();
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string Number, string keywords, [DefaultValue(-1)]int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmmeterBll bll = new Am_AmmeterBll();
                var ListData = bll.GetPageList(ref jqgridparam, Number, keywords, Stuts, ProvinceId, CityId, CountyId);
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
        public void ExportExcel([DefaultValue(-1)]int Stuts, string keywords, string Number, string ProvinceId, string CityId, string CountyId)
        {
            Am_AmmeterBll bll = new Am_AmmeterBll();
            var ListData = bll.GetPageList(keywords, Number, Stuts, ProvinceId, CityId, CountyId);
            var newlist = new List<Am_AmmeterNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmmeterNew();
                model.Address = item.Address;
                model.AmmeterMoney = item.AmmeterMoney.Value.ToString("0.00");
                model.AmmeterType_Name = item.AmmeterType_Name;
                model.AM_Code = item.AM_Code;
                model.Cell = item.Cell;
                model.City = item.City;
                model.Collector_Code = item.Collector_Code;
                model.County = item.County;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.FirstAlarm = item.FirstAlarm.ToString();
                model.Floor = item.Floor;
                model.Money = item.Money.Value.ToString("0.00");
                model.Province = item.Province;
                model.Room = item.Room;
                model.StatusStr = item.StatusStr;
                model.UserName = item.UserName;
                model.UY_Name = item.UY_Name;
                model.UY_UserName = item.UY_UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "电表号:AM_Code", "电表类型:AmmeterType_Name", "价格:AmmeterMoney_Name",
                "采集器编号:Collector_Code","租户帐号:UserName","租户姓名:U_Name","省:Province",
                "市:City", "区:County","单元:Cell","楼层:Floor","房号:Room", "地址:Address",
                "状态:StatusStr","安装时间:CreateTime", "一级预警:FirstAlarm", "电表余额:Money",
                "业主帐号:UY_UserName","业主姓名:UY_Name" };
            DeriveExcel.ListToExcel<Am_AmmeterNew>(newlist, columns, "电表数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
        /// <summary>
        /// 返回 树JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson(string Number)
        {
            IDatabase database = DataFactory.Database();
            string sql = "select Number,CollectorCode from Am_Collector where 1=1";
            //用户限定
            if (ManageProvider.Provider.Current().DepartmentId == "运营商")
            {
                sql += " and UNumber = '" + ManageProvider.Provider.Current().CompanyId + "'";
            }
            if (!StringHelper.IsNullOrEmpty(Number))
            {
                sql += " and Collector_Number = '" + Number + "'";
            }
            var list = database.FindListBySql<Am_Collector>(sql);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            TreeJsonEntity tree = new TreeJsonEntity();
            tree.id = "";
            tree.text = "所有采集器";
            tree.parentId = "0";
            tree.Attribute = "Type";
            tree.AttributeValue = "Parent";
            tree.isexpand = true;
            tree.complete = true;
            tree.hasChildren = true;
            tree.img = "/Content/Images/Icon16/folder.png";
            TreeList.Add(tree);
            foreach (var item in list)
            {
                if (item != null)
                {
                    TreeJsonEntity tree1 = new TreeJsonEntity();
                    tree1.id = item.Number;
                    tree1.text = item.CollectorCode;
                    tree1.parentId = "";
                    tree1.Attribute = "Type";
                    tree1.AttributeValue = "Collector";
                    tree1.isexpand = true;
                    tree1.complete = true;
                    tree1.hasChildren = false;
                    tree1.img = "/Content/Images/Icon16/report.png";
                    TreeList.Add(tree1);
                }
            }
            return Content(TreeList.TreeToJson());
        }
    }
}