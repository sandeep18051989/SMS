using System.Web.Mvc;
using System.Web.Routing;
using EF.Core;
using EF.Services;

namespace SMS.Controllers
{
	public abstract partial class PublicHttpController : CMSController
	{
		protected virtual ActionResult InvokeHttp404()
		{
			IController errorController = ContextHelper.Current.Resolve<CommonController>();

			var routeData = new RouteData();
			routeData.Values.Add("controller", "Common");
			routeData.Values.Add("action", "PageNotFound");

			errorController.Execute(new RequestContext(this.HttpContext, routeData));

			return new EmptyResult();
		}

	}
}
