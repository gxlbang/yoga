using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace FJCWebApp.CommonClass
{
    public class MyRoute : RouteBase
    {
        private string[] urls;
        public MyRoute(params string[] targetUrls) { urls = targetUrls; }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData result = null;
            string requestURL = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;
            requestURL = requestURL.Substring(2).Trim('/');
            if (requestURL.Contains(urls.ToArray().GetValue(0).ToString()))
            {
                requestURL = requestURL.Substring(requestURL.LastIndexOf('/') + 1);
                requestURL = requestURL.Replace(".html", "");
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Ui");
                result.Values.Add("action", urls[0]);
                result.Values.Add("Number", requestURL);
            }
            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}