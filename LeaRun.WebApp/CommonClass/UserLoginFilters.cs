using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp
{
    public class UserLoginFilters : ActionFilterAttribute
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
            else
            {
                if (Cookie != null)
                {
                    string str = Cookie.Value;
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = Utilities.DESEncrypt.Decrypt(str);
                        string[] user = str.Split('&');
                        DataAccess.IDatabase database = Repository.DataFactory.Database();
                        var model = database.FindEntity<Entity.Ho_PartnerUser>(user[0]);

                        if (model.Status==0)
                        {
                            filterContext.Result = new ContentResult()
                            {
                                Content = "<script type='text/javascript'>alert('请先完善个人信息！');location.href='/Info/Perfect';</script>",
                            };
                        }
                        else if (model.Status==1|| model.Status == 2)
                        {
                            filterContext.Result = new ContentResult()
                            {
                                Content = "<script type='text/javascript'>location.href='/Info/Information';</script>",
                            };
                        }

                    }
                }
            }
        }
    }
}