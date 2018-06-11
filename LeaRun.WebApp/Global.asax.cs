using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace LeaRun.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 当前应用程序启动这件事会发生
        /// </summary>
        protected void Application_Start()
        {
            //设置当前数据库类型
            DbHelper.DbType = (DatabaseType)Enum.Parse(typeof(DatabaseType), ConfigHelper.AppSettings("ComponentDbType"), true);
            Application["OnLineCount"] = 50;//在应用程序第一次启动时初始化在线人数为0
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WeixinConfig.Register();

        }
        /// <summary>
        /// 离开应用程序启动这件事会发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["OnLineCount"] = (int)Application["OnLineCount"] - 1;
            Application.UnLock();
        }
        /// <summary>
        /// 一个会话启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["OnLineCount"] = (int)Application["OnLineCount"] + 1;
            Application.UnLock();
        }
        ///// <summary>
        ///// 异常处理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    var isDebug = true;
        //    if (isDebug)
        //    {
        //        Exception ex = this.Context.Server.GetLastError();
        //        //var httpException = ex as HttpException ?? new HttpException(500, "服务器内部错误", ex);
        //        if (ex != null)
        //        {
        //            //登录是否过期
        //            if (!ManageProvider.Provider.IsOverdue())
        //            {
        //                //HttpContext.Current.Response.Redirect("~/Login/Default");
        //                const string url1 = "<script>window.location.href='/Login/Default'</script>";

        //                System.Web.HttpContext.Current.Response.Write(url1);

        //                System.Web.HttpContext.Current.Response.End();
        //            }
        //            Dictionary<string, string> modulesError = new Dictionary<string, string>();
        //            modulesError.Add("发生时间", DateTime.Now.ToString());
        //            modulesError.Add("错误描述", ex.Message.Replace("\r\n", ""));
        //            modulesError.Add("错误对象", ex.Source);
        //            modulesError.Add("错误页面", "" + HttpContext.Current.Request.Url + "");
        //            modulesError.Add("浏览器IE", HttpContext.Current.Request.UserAgent);
        //            modulesError.Add("服务器IP", NetHelper.GetIPAddress());
        //            Application["error"] = modulesError;
        //            //HttpContext.Current.Response.Redirect("~/Error/Index");
        //            const string url = "<script>window.location.href='/Error/Index'</script>";

        //            System.Web.HttpContext.Current.Response.Write(url);

        //            System.Web.HttpContext.Current.Response.End();
        //        }
        //    }
        //    else
        //    {
        //        const string url = "<script>window.location.href='/Error/ErrorEx'</script>";

        //        System.Web.HttpContext.Current.Response.Write(url);

        //        System.Web.HttpContext.Current.Response.End();
        //    }
        //}
    }
}