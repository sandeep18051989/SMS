using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using EF.Core;

namespace EF.Services.Service
{
	public class RouteRegistrar : IRouteRegistrar
	{
        protected readonly ITypeFinder typeFinder;

        public RouteRegistrar(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        public virtual void RegisterRoutes(RouteCollection routes)
        {
            var routeProviderTypes = typeFinder.FindClassesOfType<IRouteProvider>();
            var routeProviders = new List<IRouteProvider>();
            foreach (var providerType in routeProviderTypes)
            {
                var provider = Activator.CreateInstance(providerType) as IRouteProvider;
                routeProviders.Add(provider);
            }
            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();
            routeProviders.ForEach(rp => rp.RegisterRoutes(routes));
        }
    }
}
