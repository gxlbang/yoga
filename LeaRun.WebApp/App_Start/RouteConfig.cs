using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcDomainRouting.Code;
using LeaRun.DataAccess;
using LeaRun.Repository;
using LeaRun.Entity;

namespace LeaRun.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //IDatabase database = DataFactory.Database();
            //var clist = database.FindList<Fx_ProductClass>();
            //foreach (var item in clist)
            //{
            //    routes.Add(item.Number, new DomainRoute(
            //    item.ClassPath,  //单独设置的域名
            //    "{action}/{id}",
            //    new { controller = "Ui", action = item.ClassUrl, id = UrlParameter.Optional } //单独指向的页面
            //));
            //}
            routes.MapRoute(
                "Default",                                                                      //路由名称
                "{controller}/{action}/{id}",                                                   //带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }       //参数默认值
            );
        }
    }
}