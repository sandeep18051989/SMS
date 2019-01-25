using EF.Services.Service;
using System.Web.Mvc;
using System.Web.Routing;
namespace EF.Facebook
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("EF.Facebook.Login",
                 "Facebook/Login",
                 new { controller = "SocialAuthFacebook", action = "Login" },
                 new[] { "EF.Facebook.Controllers" }
            );

            routes.MapRoute("EF.Facebook.LoginCallback",
                 "Facebook/LoginCallback",
                 new { controller = "SocialAuthFacebook", action = "LoginCallback" },
                 new[] { "EF.Facebook.Controllers" }
            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
