using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.HouseModule
{
    public class HouseModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HouseModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HouseModule_default",
                "HouseModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
