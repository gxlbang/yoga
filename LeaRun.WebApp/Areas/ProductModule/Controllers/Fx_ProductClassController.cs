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

namespace LeaRun.WebApp.Areas.ProductModule.Controllers
{
    /// <summary>
    /// Fx_ProductClass控制器
    /// </summary>
    public class Fx_ProductClassController : PublicController<Fx_ProductClass>
    {
        Fx_ProductClassBll bll = new Fx_ProductClassBll();

        /// <summary>
        /// 返回产品栏目列表JSON
        /// </summary>
        /// <param name="jqgridparam">表格参数</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam,string Keyword)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = bll.GetPageList(ref jqgridparam);
                var ListData = bll.GetPageList1(ref jqgridparam,Keyword);
                var newlist = new List<Fx_ProductClass>();
                foreach (var item in ListData)
                {
                    if (item.IsShow == 1 && ManageProvider.Provider.Current().InnerUser > 2)
                    {
                        continue; //非管理员看不到此项,2为管理员
                    }
                    item.ClassUrl = "http://" + item.ClassPath + "/" + item.ClassUrl + "?p=" + item.Number + "&u=" + ManageProvider.Provider.Current().UserId;
                    newlist.Add(item);
                }
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = newlist
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
        /// 【产品栏目管理】提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="pclass">栏目信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue, Fx_ProductClass pclass, string BuildFormJson)
        {
            //string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                pclass.ClassText = Server.UrlDecode(pclass.ClassText);
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    pclass.Modify(KeyValue);
                    //pclass.ClassUrl = "/Ui/Index?p=" + pclass.Number;
                    database.Update(pclass, isOpenTrans);
                }
                else //新建栏目
                {
                    pclass.Create();
                    //pclass.ClassUrl = "/Ui/"+pclass.ClassUrl+"?p=" + pclass.Number;
                    database.Insert(pclass, isOpenTrans);
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
        /// 获取产品栏目对象返回JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetPclassForm(string KeyValue)
        {
            Fx_ProductClass base_user = DataFactory.Database().FindEntity<Fx_ProductClass>(KeyValue);
            if (base_user == null)
            {
                return Content("");
            }
            string strJson = base_user.ToJson();
            strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }
    }
}