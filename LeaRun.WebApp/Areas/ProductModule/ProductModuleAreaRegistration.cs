using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.ProductModule
{
    public class ProductModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ProductModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ProductModule_default",
                "ProductModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
