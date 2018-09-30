using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMS
{
    public static class FriendlyRouteExtensions
	{
        public static Route MapFriendlyRoute(this RouteCollection routes, string name, string url)
        {
            return MapFriendlyRoute(routes, name, url, null, (object)null);
        }
        public static Route MapFriendlyRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return MapFriendlyRoute(routes, name, url, defaults, (object)null);
        }
        public static Route MapFriendlyRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return MapFriendlyRoute(routes, name, url, defaults, constraints, null);
        }
        public static Route MapFriendlyRoute(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return MapFriendlyRoute(routes, name, url, null, null, namespaces);
        }
        public static Route MapFriendlyRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return MapFriendlyRoute(routes, name, url, defaults, null, namespaces);
        }
        public static Route MapFriendlyRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var route = new FriendlyRoutes(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }
    }
}