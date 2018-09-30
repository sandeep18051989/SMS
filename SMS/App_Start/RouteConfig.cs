using System.Web.Mvc;
using System.Web.Routing;
using EF.Services.Service;

namespace SMS
{
	public class RouteConfig : IRouteProvider
	{
		public void RegisterRoutes(RouteCollection routes)
		{
			//routes.MapFriendlyRoute(
		 //"Default", // Route name
		 //"{controller}/{action}/{id}", // URL with parameters
		 //new { controller = "Home", action = "Index", id = UrlParameter.Optional },
		 //new[] { "SMS.Controllers" });

			routes.MapFriendlyRoute(
						 name: "Login",
						 url: "login",
						 defaults: new { controller = "Account", action = "Login", ReturnUrl = UrlParameter.Optional },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			#region Product Routes

			routes.MapFriendlyRoute(
						 name: "HomeProduct",
						 url: "product/index",
						 defaults: new { controller = "Product", action = "Index" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
						 name: "Index",
						 url: "home/index",
						 defaults: new { controller = "Home", action = "Index" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			//routes.MapFriendlyRoute(
			//			 name: "HomeLogo",
			//			 url: "home/logo",
			//			 defaults: new { controller = "Home", action = "Logo" },
			//			 namespaces: new string[] { "SMS.Controllers" }
			//	  ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
						 name: "HomeEvent",
						 url: "event/index",
						 defaults: new { controller = "Event", action = "Index" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			#endregion

			routes.MapFriendlyRoute(
						 name: "Feedback",
						 url: "query",
						 defaults: new { controller = "Feedback", action = "Query" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			//routes.MapFriendlyRoute(
			//			 name: "FooterProductListColumn1",
			//			 url: "Product/FooterProductListColumn1",
			//			 defaults: new { controller = "Product", action = "FooterProductListColumn1" },
			//			 namespaces: new string[] { "SMS.Controllers" }
			//	  ).DataTokens.Remove("area");

			//routes.MapFriendlyRoute(
			//			 name: "FooterProductListColumn2",
			//			 url: "product/footerproductlistcolumn2",
			//			 defaults: new { controller = "Product", action = "FooterProductListColumn2" },
			//			 namespaces: new string[] { "SMS.Controllers" }
			//	  ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
						 name: "Developer",
						 url: "account/developer",
						 defaults: new { controller = "Account", action = "Developer" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
					name: "Products",
					url: "product/list",
					defaults: new { controller = "Product", action = "List" },
					namespaces: new string[] { "SMS.Controllers" }
			 ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
					name: "About",
					url: "about",
					defaults: new { controller = "Common", action = "About" },
					namespaces: new string[] { "SMS.Controllers" }
			 ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
						 name: "Userlinks",
						 url: "common/userlinks",
						 defaults: new { controller = "Common", action = "UserLinks" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
						 name: "Slider",
						 url: "slider/index",
						 defaults: new { controller = "Slider", action = "Index" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
						 name: "Register",
						 url: "account/register",
						 defaults: new { controller = "Account", action = "Register" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
						 name: "AdminDashboard",
						 url: "admin/dashboard/index",
						 defaults: new { controller = "Dashboard", action = "Index" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "Logout",
						 url: "admin/dashboard/logout",
						 defaults: new { controller = "Dashboard", action = "LogOff" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "SliderSettings",
						 url: "admin/slider/index",
						 defaults: new { controller = "Slider", action = "Index" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "EventList",
						 url: "admin/event/list",
						 defaults: new { controller = "Event", action = "List" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "ProductList",
						 url: "admin/product/list",
						 defaults: new { controller = "Product", action = "List" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "TokenList",
						 url: "admin/datatoken/list",
						 defaults: new { controller = "DataToken", action = "List" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "TemplateList",
						 url: "admin/template/templateList",
						 defaults: new { controller = "Template", action = "TemplateList" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "CustomPageList",
						 url: "admin/customPage/list",
						 defaults: new { controller = "CustomPage", action = "List" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "FeedbackList",
						 url: "admin/feedback/feedbacklist",
						 defaults: new { controller = "Feedback", action = "FeedbackList" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "PermissionsList",
						 url: "admin/permission/permissionsList",
						 defaults: new { controller = "Permission", action = "Permissions" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "Roles",
						 url: "admin/role/list",
						 defaults: new { controller = "Role", action = "List" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapFriendlyRoute(
						 name: "Users",
						 url: "admin/user/list",
						 defaults: new { controller = "User", action = "List" },
						 namespaces: new string[] { "SMS.Areas.Admin.Controllers" }
				  ).DataTokens.Add("area", "Admin");

			routes.MapRoute(name: "Installation", url: "install", defaults: new { controller = "Install", action = "Index" }, namespaces: new string[] { "SMS.Controllers" }).DataTokens.Remove("area");

			routes.MapFriendlyRoute(
				 name: "PageNotFound",
						 url: "page-not-found",
						 defaults: new { controller = "Common", action = "PageNotFound" },
						 namespaces: new string[] { "SMS.Controllers" }
				  ).DataTokens.Remove("area");

			//robots.txt
			routes.MapRoute("robots.txt",
								 "robots.txt",
								 new { controller = "Common", action = "RobotsTextFile" },
								 new[] { "SMS.Controllers" });

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
