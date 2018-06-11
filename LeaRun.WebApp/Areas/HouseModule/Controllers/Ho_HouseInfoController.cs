/*
* 姓名:gxlbang
* 类名:Ho_HouseInfo
* CLR版本：
* 创建时间:2017-11-24 10:54:56
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
using LeaRun.Entity.EntityModel;
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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.HouseModule.Controllers
{
    /// <summary>
    /// Ho_HouseInfo控制器
    /// </summary>
    public class Ho_HouseInfoController : PublicController<Ho_HouseInfo>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keyword)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_HouseInfoBll bll = new Ho_HouseInfoBll();
                var ListData = bll.GetPageList(ref jqgridparam, keyword);
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
        /// 首页
        /// </summary>
        /// <returns></returns>
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("146f3b69-db96-4f0b-83db-64cd87f9f8e2");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
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
        public ActionResult SubmitUserForm(string KeyValue, Ho_HouseInfo model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                model.Status = model.StatusStr == "在售" ? 1 : 0;
                model.Video = Server.UrlDecode(model.Video);
                var list = new List<Ho_HouseImage>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    #region 更新封面
                    //获得实体类
                    var item = database.FindEntity<Ho_HouseInfo>(KeyValue);
                    //更新图册
                    StringBuilder whereStr = new StringBuilder();
                    whereStr.AppendFormat("'{0}'", item.DesignImageNumber);
                    whereStr.AppendFormat(",'{0}'", item.RealImageNumber);
                    whereStr.AppendFormat(",'{0}'", item.HouseImageNumber);
                    whereStr.AppendFormat(",'{0}'", item.HouseTypeImageNumber);
                    //一次性获取四个封面图
                    var imglist = database.FindList<Ho_HouseImage>(" and Number in(" + whereStr.ToString() + ")");
                    //效果图册
                    var DesignImage = imglist.FirstOrDefault<Ho_HouseImage>(o => o.Number == item.DesignImageNumber);
                    DesignImage.ImageUrl = model.DesignImage;
                    list.Add(DesignImage);
                    //实景图册
                    var RealImage = imglist.FirstOrDefault<Ho_HouseImage>(o => o.Number == item.RealImageNumber);
                    RealImage.ImageUrl = model.RealImage;
                    list.Add(RealImage);
                    //样板图册
                    var HouseImage = imglist.FirstOrDefault<Ho_HouseImage>(o => o.Number == item.HouseImageNumber);
                    HouseImage.ImageUrl = model.HouseImage;
                    list.Add(HouseImage);
                    //户型图册
                    var HouseTypeImage = imglist.FirstOrDefault<Ho_HouseImage>(o => o.Number == item.HouseTypeImageNumber);
                    HouseTypeImage.ImageUrl = model.HouseTypeImage;
                    list.Add(HouseTypeImage);

                    database.Update<Ho_HouseImage>(list, isOpenTrans);
                    #endregion
                    model.Modify(KeyValue);
                    database.Update(model, isOpenTrans);

                }
                else //新建
                {
                    model.Create();
                    #region 图册添加
                    //效果图册
                    var DesignImage = new Ho_HouseImage()
                    {
                        GroupName = "效果图册",
                        GroupNumber = "5f69b268-504e-44f8-950b-dfda8e00ee19",
                        Orders = 0,
                        IsDel = 0,
                        HouseNumber = model.Number,
                        HouseName=model.Name,
                        ImageTitle = "封面图片",
                        ImageUrl = model.DesignImage,
                        ImageDes = "封面图册"
                    };
                    DesignImage.Create();
                    list.Add(DesignImage);
                    model.DesignImageNumber = DesignImage.Number;
                    //实景图册
                    var RealImage = new Ho_HouseImage()
                    {
                        GroupName = "实景图册",
                        GroupNumber = "bc4a267a-3404-4e2e-9053-a0788bd53789",
                        Orders = 0,
                        IsDel = 0,
                        HouseNumber = model.Number,
                        HouseName = model.Name,
                        ImageTitle = "封面图片",
                        ImageUrl = model.RealImage,
                        ImageDes = "封面图册"
                    };
                    RealImage.Create();
                    list.Add(RealImage);
                    model.RealImageNumber = RealImage.Number;

                    //样板图册
                    var HouseImage = new Ho_HouseImage()
                    {
                        GroupName = "样板图册",
                        GroupNumber = "b8af3e9b-aea0-4618-9f66-67987b081731",
                        Orders = 0,
                        IsDel = 0,
                        HouseNumber = model.Number,
                        HouseName = model.Name,
                        ImageTitle = "封面图片",
                        ImageUrl = model.HouseImage,
                        ImageDes = "封面图册"
                    };
                    HouseImage.Create();
                    list.Add(HouseImage);
                    model.HouseImageNumber = HouseImage.Number;

                    //户型图册
                    var HouseTypeImage = new Ho_HouseImage()
                    {
                        GroupName = "户型图册",
                        GroupNumber = "e6b2a2ee-9744-4902-9ad1-d29b06c883ab",
                        Orders = 0,
                        IsDel = 0,
                        HouseNumber = model.Number,
                        HouseName = model.Name,
                        ImageTitle = "封面图片",
                        ImageUrl = model.HouseTypeImage,
                        ImageDes = "封面图册"
                    };
                    HouseTypeImage.Create();
                    list.Add(HouseTypeImage);
                    model.HouseTypeImageNumber = HouseTypeImage.Number;

                    database.Insert<Ho_HouseImage>(list, isOpenTrans);
                    #endregion
                    var result = database.Insert(model, isOpenTrans);
                    //创建图册
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

        public ActionResult Pictures(string HouseNumber, string GroupNumber)
        {
            string _ModuleId = DESEncrypt.Encrypt("1020da1a-b5b4-40c6-bab9-95fe0a0ddc6d");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }

        #region 文件上传、删除
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="Filedata"></param>
        /// <returns></returns>
        public ActionResult ImgUpload(HttpPostedFileBase Filedata)
        {
            try
            {
                FileProperty fileproperty = new FileProperty();
                //没有文件上传，直接返回
                if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
                {
                    return HttpNotFound();
                }
                //获取文件完整文件名(包含绝对路径)
                //文件存放路径格式：/Resource/Document/Email/{日期}/{guid}.{后缀名}
                //例如：/Resource/Document/Email/20130913/43CA215D947F8C1F1DDFCED383C4D706.jpg
                string fileGuid = CommonHelper.GetGuid;
                long filesize = Filedata.ContentLength;
                string FileEextension = Path.GetExtension(Filedata.FileName);
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");

                string virtualPath = string.Format("/Resource/ProductImages/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                string fullFileName = this.Server.MapPath(virtualPath);
                //创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                if (!System.IO.File.Exists(fullFileName))
                {
                    Filedata.SaveAs(fullFileName);
                    fileproperty.Id = fileGuid;
                    fileproperty.Name = Filedata.FileName;
                    fileproperty.Eextension = FileEextension;
                    fileproperty.CreateDate = DateTime.Now;
                    fileproperty.Path = virtualPath;
                    fileproperty.Size = SizeHelper.CountSize(filesize);
                }
                return Content(fileproperty.ToJson());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns></returns>
        public ActionResult ImgDeleteFile(string Path)
        {
            try
            {
                string FilePath = this.Server.MapPath(Path);
                if (System.IO.File.Exists(FilePath))
                    System.IO.File.Delete(FilePath);
                return Content(new JsonMessage { Success = true, Code = "1", Message = "删除成功" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion
        /// <summary>
        /// 获得业务区域
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public ActionResult GetCityListJson(string ParentId) {
            IDatabase database = DataFactory.Database();
            var ListData = database.FindList<Ho_CityInfo>();
            return Content(ListData.ToJson());
        }

    }
}