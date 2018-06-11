using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaRun.WebApp._01_Extension
{
    /// <summary>
    /// UploadweixinImgHandler 的摘要说明
    /// </summary>
    public class UploadweixinImgHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String callback = context.Request.QueryString["CKEditorFuncNum"].ToString();
            ///'遍历File表单元素
            HttpFileCollection files = HttpContext.Current.Request.Files;
            for (int iFile = 0; iFile < files.Count; iFile++)
            {
                //    ///'检查文件扩展名字
                HttpPostedFile postedFile = files[iFile];
                //HttpPostedFile postedFile = files[0];
                string fileName;   //, fileExtension
                fileName = System.IO.Path.GetFileName(postedFile.FileName);


                string fileContentType = postedFile.ContentType.ToString();
                if (fileContentType == "image/bmp" || fileContentType == "image/gif" ||
                    fileContentType == "image/png" || fileContentType == "image/x-png" || fileContentType == "image/jpeg"
                    || fileContentType == "image/pjpeg")
                {
                    if (postedFile.ContentLength <= 2097152)
                    {
                        string filepath = postedFile.FileName;      //得到的是文件的完整路径,包括文件名，如：C:\Documents and Settings\Administrator\My Documents\My Pictures\20022775_m.jpg
                        //string filepath = FileUpload1.FileName;               //得到上传的文件名20022775_m.jpg

                        string serverpath = context.Server.MapPath("~/Images/") + fileName;//取得文件在服务器上保存的位置C:\Inetpub\wwwroot\WebSite1\images\20022775_m.jpg

                        postedFile.SaveAs(serverpath);//上传图片到服务器指定地址

                        string imageurl = "/UpLoads/Images/" + fileName;//我是将测试时的本地地址+放置图像的文件夹+图片名称作为返回的URL

                        // 返回"图像"选项卡并显示图片
                        context.Response.Write("<script type=\"text/javascript\">");
                        context.Response.Write("window.parent.CKEDITOR.tools.callFunction(" + callback
                               + ",'" + imageurl + "','')");
                        context.Response.Write("</script>");
                    }
                    else
                    {
                        context.Response.Write("<script>alert('上传文件不能大于2M！')</script>");
                    }
                }
                else
                {
                    context.Response.Write("<script>alert('只支持BMP、GIF、JPG、PNG格式的图片！')</script>");
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}