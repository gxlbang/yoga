/*
* 姓名:gxlbang
* 类名:Fx_NewsClass
* CLR版本：
* 创建时间:2017-11-27 16:17:52
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

namespace LeaRun.WebApp.Areas.NewsModule.Controllers
{
    /// <summary>
    /// Fx_NewsClass控制器
    /// </summary>
    public class Fx_NewsClassController : PublicController<Fx_NewsClass>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string Keyword, string Number)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Fx_NewsClassBll bll = new Fx_NewsClassBll();
                var ListData = bll.GetPageList(ref jqgridparam, Keyword, Number);
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
        /// 返回 树JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson(string Number)
        {
            IDatabase database = DataFactory.Database();
            string sql = "select Number,Name,ClassPic,ParenNumber,ParenName,ClassOrder,ClassUrl,StatusStr,Status,Remark from Fx_NewsClass where 1=1";
            if (!StringHelper.IsNullOrEmpty(Number))
            {
                sql += " and ParenNumber = '" + Number + "'";
            }
            var list = database.FindListBySql<Fx_NewsClass>(sql);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            TreeJsonEntity tree = new TreeJsonEntity();
            tree.id = "x999";
            tree.text = "根栏目";
            tree.parentId = "0";
            tree.Attribute = "Type";
            tree.AttributeValue = "Parent";
            tree.isexpand = true;
            tree.complete = true;
            tree.hasChildren =  true; 
            tree.img = "/Content/Images/Icon16/folder.png";
            TreeList.Add(tree);
            foreach (var item in list)
            {
                if (item != null)
                {
                    TreeJsonEntity tree1 = new TreeJsonEntity();
                    tree1.id = item.Number;
                    tree1.text = item.Name;
                    tree1.parentId = item.ParenNumber=="0"? "x999" : item.ParenNumber;
                    tree1.Attribute = "Type";
                    tree1.AttributeValue = "NewsClass";
                    tree1.isexpand = true;
                    tree1.complete = true;
                    tree1.hasChildren = item.IsHasChild == 0 ? false : true; //0为没有,其他则有
                    tree1.img = "/Content/Images/Icon16/report.png";
                    TreeList.Add(tree1);
                }
            }
            return Content(TreeList.TreeToJson());
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="pclass">栏目信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue, Fx_NewsClass model, string BuildFormJson)
        {
            //string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                model.Class_Content = Server.UrlDecode(model.Class_Content);
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    model.StatusStr = model.Status == 1 ? "正常" : "禁用";
                    model.Modify(KeyValue);
                    database.Update(model, isOpenTrans); 
                }
                else //新建栏目
                {
                    model.Create();
                    model.ClassDepth = 1 ;
                    model.StatusStr = model.Status == 1 ? "正常" : "禁用";
                    if (model.ParenNumber != "0") {
                        var pmodel = database.FindEntity<Fx_NewsClass>(model.ParenNumber);
                        model.ClassDepth = pmodel.ClassDepth + 1;
                        pmodel.IsHasChild = 1;
                        database.Update(pmodel,isOpenTrans);
                    }
                   int result= database.Insert(model, isOpenTrans);
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
    }
}