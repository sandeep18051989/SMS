using System.Web.Mvc;

namespace SMS.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.Clear();
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                      name: "Root",
                      url: "Home/Index",
                      defaults: new { controller = "Home", action = "Index" },
                      namespaces: new string[] { "SMS.Controllers" }
                 ).DataTokens.Remove("area");

        }
    }
}