using EF.Core.Social;
using System.Web.Routing;
namespace EF.Services.Social
{
    public partial interface ISocialAuthenticationMethod : ISocial
    {
        void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues);

        void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues);

        void Install();

        void Uninstall();
    }
}
