using LeaRun.Utilities;
using LeaRun.WebApp.Areas.SystemModule.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.SystemModule.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImageUpload(string id, string name,string type, string lastModifiedDate, int size, HttpPostedFileBase file)
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
            return Json(new
            {
                jsonrpc = "2.0",
                id = id,
                filePath = filePath
            });
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Content("没有文件！", "text/plain");
            }
            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
            try
            {
                file.SaveAs(fileName);
                return Content("成功 ！", "text/plain");
            }
            catch
            {
                return Content("上传异常 ！", "text/plain");
            }
        }
        [HttpPost]
        public ActionResult UploadImage()
        {
            string savePath = "/Resource/UploadImages/";
            string saveUrl = "/Resource/UploadImages/";
            string fileTypes = "xlsx,zip,rar,ppt,doc,xls";
            int maxSize = 50000000;

            Hashtable hash = new Hashtable();

            HttpPostedFileBase file = Request.Files["imgFile"];
            if (file == null)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "请选择文件";
                return Json(hash);
            }

            string dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传目录不存在";
                return Json(hash);
            }

            string fileName = file.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));

            if (file.InputStream == null || file.InputStream.Length > maxSize)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传文件大小超过限制";
                return Json(hash);
            }

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传文件扩展名是不允许的扩展名";
                return Json(hash);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo) + fileExt;
            string filePath = dirPath + newFileName;
            file.SaveAs(filePath);
            string fileUrl = saveUrl + newFileName;

            hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;

            return Json(hash, "text/html;charset=UTF-8"); ;

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