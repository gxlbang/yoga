using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.WebModule
{
    public class WebModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WebModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WebModule_default",
                "WebModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
