using System;
using System.Web;
using System.Web.Routing;
using EF.Core;
using EF.Data;
using EF.Services.Http;
using EF.Services.Service;

namespace SMS.App_Start
{
	public partial class SeoRoutes : FriendlyRoutes
	{
		public SeoRoutes(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
		}

		public SeoRoutes(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
		}

		public SeoRoutes(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
				: base(url, defaults, constraints, routeHandler)
		{
		}

		public SeoRoutes(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
				: base(url, defaults, constraints, dataTokens, routeHandler)
		{
		}

		public override RouteData GetRouteData(HttpContextBase httpContext)
		{
			RouteData data = base.GetRouteData(httpContext);
			if (data != null && DatabaseHelper.DatabaseIsInstalled())
			{
				var urlRecordService = ContextHelper.Current.Resolve<IUrlService>();
				var slug = data.Values["name"] as string;
				var urlRecord = urlRecordService.GetBySlug(slug);
				if (urlRecord == null)
				{
					data.Values["controller"] = "Common";
					data.Values["action"] = "PageNotFound";
					return data;
				}
				if (!urlRecord.IsActive)
				{
					var activeSlug = urlRecordService.GetBySlug(urlRecord.Slug);
					if (string.IsNullOrWhiteSpace(activeSlug.Slug))
					{
						data.Values["controller"] = "Common";
						data.Values["action"] = "PageNotFound";
						return data;
					}

					var webHelper = ContextHelper.Current.Resolve<IUrlHelper>();
					var response = httpContext.Response;
					response.Status = "301 Moved Permanently";
					response.RedirectLocation = string.Format("{0}{1}", webHelper.GetLocation(false), activeSlug);
					response.End();
					return null;
				}

				var slugForCurrentLanguage = UrlExtensions.GetSystemName(urlRecord.EntityId, urlRecord.EntityName);
				if (!String.IsNullOrEmpty(slugForCurrentLanguage) &&
					 !slugForCurrentLanguage.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
				{
					var webHelper = ContextHelper.Current.Resolve<IUrlHelper>();
					var response = httpContext.Response;
					response.Status = "302 Moved Temporarily";
					response.RedirectLocation = string.Format("{0}{1}", webHelper.GetLocation(false), slugForCurrentLanguage);
					response.End();
					return null;
				}

				switch (urlRecord.EntityName.ToLowerInvariant())
				{
					case "school":
						{
							data.Values["controller"] = "School";
							data.Values["action"] = "Details";
							data.Values["id"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
					case "userinfo":
						{
							data.Values["controller"] = "Account";
							data.Values["action"] = "Details";
							data.Values["userid"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
					case "teacher":
						{
							data.Values["controller"] = "Teacher";
							data.Values["action"] = "Details";
							data.Values["teacherid"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
					case "student":
						{
							data.Values["controller"] = "Student";
							data.Values["action"] = "Details";
							data.Values["studentid"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
					case "product":
						{
							data.Values["controller"] = "Product";
							data.Values["action"] = "Detail";
							data.Values["id"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
                    case "productcategory":
                        {
                            data.Values["controller"] = "ProductCategory";
                            data.Values["action"] = "Detail";
                            data.Values["id"] = urlRecord.EntityId;
                            data.Values["name"] = urlRecord.Slug;
                        }
                        break;
                    case "news":
						{
							data.Values["controller"] = "News";
							data.Values["action"] = "Details";
							data.Values["newsid"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
					case "event":
						{
							data.Values["controller"] = "Event";
							data.Values["action"] = "Details";
							data.Values["eventid"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
					case "custompage":
						{
							data.Values["controller"] = "Page";
							data.Values["action"] = "PageDetails";
							data.Values["custompageid"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
					case "blog":
						{
							data.Values["controller"] = "Blog";
							data.Values["action"] = "Details";
							data.Values["blogid"] = urlRecord.EntityId;
							data.Values["name"] = urlRecord.Slug;
						}
						break;
					default:
						break;
				}
			}
			return data;
		}
	}


}