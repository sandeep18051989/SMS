using System.Web.Routing;
using EF.Services.Service;

namespace SMS
{
	public partial class GenericUrlRouteProvider : IRouteProvider
	{
		public void RegisterRoutes(RouteCollection routes)
		{
			routes.MapGenericPathRoute("GenericUrl", "{name}", new { controller = "Common", action = "GenericUrl" }, new[] { "SMS.Controllers" });
			routes.MapFriendlyRoute("School", "{name}", new { controller = "School", action = "Details" }, new[] { "SMS.Controllers" }).DataTokens.Remove("admin");
			routes.MapFriendlyRoute("Teacher", "{name}", new { controller = "Teacher", action = "Details" }, new string[] { "SMS.Controllers" }).DataTokens.Remove("area");
			routes.MapFriendlyRoute("Student", "{name}", new { controller = "Student", action = "Details" }, new string[] { "SMS.Controllers" }).DataTokens.Remove("area");
			routes.MapFriendlyRoute("Product", "{name}", new { controller = "Product", action = "Details" }, new string[] { "SMS.Controllers" }).DataTokens.Remove("area");
			routes.MapFriendlyRoute("News", "{name}", new { controller = "News", action = "Details" }, new string[] { "SMS.Controllers" }).DataTokens.Remove("area");
			routes.MapFriendlyRoute("Event", "{name}", new { controller = "Event", action = "Details" }, new string[] { "SMS.Controllers" }).DataTokens.Remove("area");
			routes.MapFriendlyRoute("Blog", "{name}", new { controller = "Blog", action = "Details" }, new string[] { "SMS.Controllers" }).DataTokens.Remove("area");
			routes.MapFriendlyRoute("Page","{name}", new { controller = "Page", action = "PageDetails" }, new string[] { "SMS.Controllers" }).DataTokens.Remove("area");
			routes.MapFriendlyRoute("CustomPage", "{name}", new { controller = "Page", action = "PageDetails" }, new string[] { "SMS.Controllers" }).DataTokens.Remove("area");
		}

		public int Priority
		{
			get
			{
				return -1000000;
			}
		}
	}
}
