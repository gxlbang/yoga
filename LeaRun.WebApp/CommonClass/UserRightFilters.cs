using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp
{
    public class UserRightFilters : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //处理用户登录状态
            filterContext.Result = new ContentResult()
            {
                Content = "<script type='text/javascript'>location.href='/Ui/Index';</script>",
            };
        }
    }
}