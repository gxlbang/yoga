using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp
{
    public class UserOperatorFilters : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //处理用户登录状态
            HttpCookie Cookie = filterContext.RequestContext.HttpContext.Request.Cookies["WebUserInfo"];
            WebData webBLL = new WebData();
            if (!webBLL.ValidateLoginWebUser(Cookie))
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "<script type='text/javascript'>alert('未登录或登录超时,请重新登录!');location.href='/Account/Login';</script>",
                };
            }
            if (Cookie != null)
            {
                string str = Cookie.Value;
                if (!string.IsNullOrEmpty(str))
                {
                    str = Utilities.DESEncrypt.Decrypt(str);
                    string[] user = str.Split('&');
                    DataAccess.IDatabase database = Repository.DataFactory.Database();
                    var model = database.FindEntity<Entity.Ho_PartnerUser>(user[0]);

                    if (model.UserRole != "运营商")
                    {
                        filterContext.Result = new ContentResult()
                        {
                            Content = "<script type='text/javascript'>alert('非法访问！');location.href='/Personal/Index';</script>",
                        };
                    }

                }
            }
        }
    }
}