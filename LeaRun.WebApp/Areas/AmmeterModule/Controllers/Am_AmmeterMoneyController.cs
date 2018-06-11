/*
* 姓名:gxlbang
* 类名:Am_AmmeterMoney
* CLR版本：
* 创建时间:2018-04-14 10:54:41
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
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_AmmeterMoney控制器
    /// </summary>
    public class Am_AmmeterMoneyController : PublicController<Am_AmmeterMoney>
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
                Am_AmmeterMoneyBll bll = new Am_AmmeterMoneyBll();
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
        public ActionResult SubmitUserForm(string KeyValue, Am_AmmeterMoney model, string BuildFormJson)
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
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "电价" + Message);
                }
                else //新建
                {
                    model.Create();
                    var IsOk = database.Insert(model, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "成功" : "失败", "电价" + Message);
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
        /// <summary>
        /// 数据导出
        /// </summary>
        public void ExportExcel(string Keyword)
        {
            Am_AmmeterMoneyBll bll = new Am_AmmeterMoneyBll();
            var ListData = bll.GetPageList(Keyword);
            var newlist = new List<Am_AmmeterMoneyNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmmeterMoneyNew();
                model.Classify = item.Classify.Value==0?"单费率电价":"其他";
                model.FirstMoney = item.FirstMoney.Value.ToString("0.00");
                model.Name = item.Name;
                model.Remark = item.Remark;
                model.UserName = item.UserName;
                model.UserRealName = item.UserRealName;

                newlist.Add(model);
            }
            string[] columns = new string[] { "名称:Name", "价格类型:Classify", "费率(元/kwh):FirstMoney", "业主姓名:UserRealName",
                "业主帐号:UserName", "备注:Remark" };
            DeriveExcel.ListToExcel<Am_AmmeterMoneyNew>(newlist, columns, "电价数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}