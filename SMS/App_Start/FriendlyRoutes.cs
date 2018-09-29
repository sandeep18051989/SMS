using System.Web;
using System.Web.Routing;
using EF.Data;
namespace SMS
{
	public class FriendlyRoutes : Route
	{

		#region Constructors

		public FriendlyRoutes(string url, IRouteHandler routeHandler)
			 : base(url, routeHandler)
		{
		}

		public FriendlyRoutes(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
			 : base(url, defaults, routeHandler)
		{
		}

		public FriendlyRoutes(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
			 : base(url, defaults, constraints, routeHandler)
		{
		}

		public FriendlyRoutes(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
			 : base(url, defaults, constraints, dataTokens, routeHandler)
		{
		}

		#endregion

		#region Methods

		public override RouteData GetRouteData(HttpContextBase httpContext)
		{
			if (DatabaseHelper.DatabaseIsInstalled())
			{
				string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath;
				string applicationPath = httpContext.Request.ApplicationPath;
				if (virtualPath.IsLocalizedUrl(applicationPath, false))
				{
					string rawUrl = httpContext.Request.RawUrl;
					if (string.IsNullOrEmpty(rawUrl))
						rawUrl = "/";
					rawUrl = rawUrl.RemoveApplicationPathFromRawUrl(applicationPath);
					rawUrl = "~" + rawUrl;
					httpContext.RewritePath(rawUrl, true);
				}
			}
			RouteData data = base.GetRouteData(httpContext);
			return data;
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			VirtualPathData data = base.GetVirtualPath(requestContext, values);

			if (data != null && DatabaseHelper.DatabaseIsInstalled())
			{
				string rawUrl = requestContext.HttpContext.Request.RawUrl;
				string applicationPath = requestContext.HttpContext.Request.ApplicationPath;
				if (rawUrl.IsLocalizedUrl(applicationPath, true))
				{
					data.VirtualPath = applicationPath;
				}
			}
			return data;
		}

		#endregion
	}
}