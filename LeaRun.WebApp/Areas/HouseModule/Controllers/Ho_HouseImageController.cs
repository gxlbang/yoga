/*
* 姓名:gxlbang
* 类名:Ho_HouseImage
* CLR版本：
* 创建时间:2017-11-24 10:55:19
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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.HouseModule.Controllers
{
    /// <summary>
    /// Ho_HouseImage控制器
    /// </summary>
    public class Ho_HouseImageController : PublicController<Ho_HouseImage>
    {
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("1020da1a-b5b4-40c6-bab9-95fe0a0ddc6d");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }
        public ActionResult UploadImageIndex()
        {
            return View();
        }

        /// <summary>
        /// 获取所有图片
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPictureListJson(JqGridParam jqgridparam, string Hnumber, string Gnumber)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_HouseImageBll bll = new Ho_HouseImageBll();
                var ListData = bll.GetPageList(ref jqgridparam, Hnumber, Gnumber);
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
        /// 提交表单
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="pclass">栏目信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue, Ho_HouseImage model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //更新楼盘里的记录
                    var oldModel = database.FindEntity<Ho_HouseImage>(KeyValue);
                    if (oldModel.IsDel == 0) //不能删除的那一张为主图
                    {
                        var hmodel = database.FindEntity<Ho_HouseInfo>(oldModel.HouseNumber);
                        switch (oldModel.GroupNumber)
                        {
                            case "5f69b268-504e-44f8-950b-dfda8e00ee19": //效果图
                                hmodel.DesignImage = model.ImageUrl;
                                break;
                            case "bc4a267a-3404-4e2e-9053-a0788bd53789": //实景图
                                hmodel.RealImage = model.ImageUrl;
                                break;
                            case "b8af3e9b-aea0-4618-9f66-67987b081731": //样板
                                hmodel.HouseImage = model.ImageUrl;
                                break;
                            case "e6b2a2ee-9744-4902-9ad1-d29b06c883ab": //户型
                                hmodel.HouseTypeImage = model.ImageUrl;
                                break;
                        }
                        hmodel.Modify(oldModel.HouseNumber);
                        var result = database.Update(hmodel, isOpenTrans);
                    }
                    model.Modify(KeyValue);
                   var idex= database.Update(model, isOpenTrans);
                }
                else //新建
                {
                    model.Create();
                    var result = database.Insert(model, isOpenTrans);
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
        /// 返回 树JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson(string Number)
        {
            IDatabase database = DataFactory.Database();
            string sql = "select Number,Name from Ho_HouseInfo where 1=1";
            if (!StringHelper.IsNullOrEmpty(Number))
            {
                sql += " and Number = '" + Number + "'";
            }
            var houseList = database.FindListBySql<Ho_HouseInfo>(sql);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            foreach (var houseModel in houseList)
            {
                if (houseModel != null)
                {
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = houseModel.Number;
                    tree.text = houseModel.Name;
                    tree.parentId = "0";
                    tree.Attribute = "Type";
                    tree.AttributeValue = "House";
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = true;
                    tree.img = "/Content/Images/Icon16/house_one.png";
                    TreeList.Add(tree);

                    //剩余四个图册
                    var dicList = database.FindList<Base_DataDictionaryDetail>(" and DataDictionaryId = '9ff7250d-bb28-44ac-a721-04bf33325a85'");
                    if (dicList != null)
                    {
                        foreach (var item in dicList)
                        {
                            var treeModel = new TreeJsonEntity()
                            {
                                id = item.DataDictionaryDetailId,
                                text = item.FullName,
                                parentId = houseModel.Number,
                                value = houseModel.Number,
                                Attribute = "Type",
                                AttributeValue = "Group",
                                isexpand = true,
                                complete = true,
                                hasChildren = false,
                                img = "/Content/Images/Icon16/image.png"
                            };
                            TreeList.Add(treeModel);
                        }
                    }

                }
            }
            return Content(TreeList.TreeToJson());
        }


        /// <summary>
        /// 图片上传-支持多图
        /// </summary>
        /// <param name="id">插件自带</param>
        /// <param name="name">图片名称</param>
        /// <param name="Hnumber">楼盘编号</param>
        /// <param name="Gnumber">图册编号</param>
        /// <param name="type">上传文件类型</param>
        /// <param name="lastModifiedDate">时间</param>
        /// <param name="size">大小</param>
        /// <param name="file">上传的文件</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImageUpload(string id, string name, string Hnumber, string Gnumber, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            string filePathName = string.Empty;
            var dirtory = Server.MapPath("~/Resource");
            string localPath = Path.Combine(dirtory, "pictures");
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }
            string ex = Path.GetExtension(file.FileName);
            if (!IsImageExtensionName(ex))
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 500, message = "非图片文件" }, id = "id" });
            }
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            var filePath = "/Resource/pictures/" + filePathName;
            file.SaveAs(Path.Combine(localPath, filePathName));
            //保存图片
            if (!StringHelper.IsNullOrEmpty(Hnumber) && !StringHelper.IsNullOrEmpty(Gnumber))
            {
                var model = new Ho_HouseImage() { ImageUrl = filePath, HouseNumber = Hnumber, GroupNumber = Gnumber };
                model.Create();
                model.Orders = 0;
                model.IsDel = 1; // 1代表可删除
                IDatabase database = DataFactory.Database();
                var hmodel = database.FindEntity<Ho_HouseInfo>(Hnumber);
                if (hmodel != null) { model.HouseName = hmodel.Name; }
                var dmodel = database.FindEntityByWhere<Base_DataDictionaryDetail>(" and DataDictionaryDetailId = '" + Gnumber + "'");
                if (dmodel != null) { model.GroupName = dmodel.FullName; }
                if (hmodel != null) { model.HouseName = hmodel.Name; }
                int res = database.Insert(model);
            }
            return Json(new
            {
                jsonrpc = "2.0",
                id = id,
                filePath = filePath
            });
        }
        private bool IsImageExtensionName(string ex)
        {
            ex = ex.ToLower();
            if (ex == ".jpg" || ex == ".jpeg" || ex == ".png" || ex == ".bmp" || ex == ".gif")
                return true;
            else
                return false;
        }
    }
}