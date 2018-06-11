using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.NewsModule
{
    public class NewsModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NewsModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NewsModule_default",
                "NewsModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
