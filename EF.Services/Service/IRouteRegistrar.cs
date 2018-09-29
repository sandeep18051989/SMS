using System.Web.Routing;

namespace EF.Services.Service
{
    public interface IRouteRegistrar
	{
        void RegisterRoutes(RouteCollection routes);
    }
}
