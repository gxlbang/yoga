using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.SystemModule
{
    public class SystemModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SystemModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SystemModule_default",
                "SystemModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
