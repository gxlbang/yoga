using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule
{
    public class AmmeterModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AmmeterModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AmmeterModule_default",
                "AmmeterModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
