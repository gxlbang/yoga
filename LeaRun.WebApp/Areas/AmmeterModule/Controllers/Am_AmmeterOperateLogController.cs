/*
* 姓名:gxlbang
* 类名:Am_AmmeterOperateLog
* CLR版本：
* 创建时间:2018-04-15 14:54:26
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
    /// Am_AmmeterOperateLog控制器
    /// </summary>
    public class Am_AmmeterOperateLogController : PublicController<Am_AmmeterOperateLog>
    {
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("a8048fac-24cd-40ba-9b50-6902596ce5c9");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string Number, string keywords, string BeginTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmmeterOperateLogBll bll = new Am_AmmeterOperateLogBll();
                var ListData = bll.GetPageList(ref jqgridparam, Number, keywords, BeginTime, EndTime);
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
        public void ExportExcel(string Number, string keywords, string BeginTime, string EndTime)
        {
            Am_AmmeterOperateLogBll bll = new Am_AmmeterOperateLogBll();
            var ListData = bll.GetPageList(keywords, Number, BeginTime, EndTime);
            var newlist = new List<Am_AmmeterOperateLogNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmmeterOperateLogNew();
                model.AmmeterCode = item.AmmeterCode;
                model.CollectorCode = item.CollectorCode;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.OperateTypeStr = item.OperateTypeStr;
                model.Result = item.Result;
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "采集器:CollectorCode", "电表:AmmeterCode", "操作人:UserName",
                "操作人姓名:U_Name","操作类型:OperateTypeStr",
                "操作结果:Result","操作时间:CreateTime" };
            DeriveExcel.ListToExcel<Am_AmmeterOperateLogNew>(newlist, columns, "电表日志" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}