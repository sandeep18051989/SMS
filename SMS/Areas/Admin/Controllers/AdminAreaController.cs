using System;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Services;
using EF.Services.Http;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	[Authorize]
	public abstract partial class AdminAreaController : CMSController
	{
		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			ContextHelper.Current.Resolve<IUserContext>().IsAdmin = true;
			base.Initialize(requestContext);
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.Exception != null)
				LogException(filterContext.Exception);
			base.OnException(filterContext);
		}

		protected ActionResult AccessDeniedView()
		{
			return RedirectToAction("AccessDenied", "Permission");
		}

		protected void SaveSelectedTabName(string tabName = "", bool persistForTheNextRequest = true)
		{
			if (string.IsNullOrEmpty(tabName))
			{
				tabName = this.Request.Form["selected-tab-name"];
			}

			if (!string.IsNullOrEmpty(tabName))
			{
				const string dataKey = "nop.selected-tab-name";
				if (persistForTheNextRequest)
				{
					TempData[dataKey] = tabName;
				}
				else
				{
					ViewData[dataKey] = tabName;
				}
			}
		}

		protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return new JsonResult()
			{
				Data = data,
				ContentType = contentType,
				ContentEncoding = contentEncoding,
				JsonRequestBehavior = behavior,
				MaxJsonLength = int.MaxValue
			};
		}

		[ValidateInput(false)]
		public virtual ActionResult UrlReservedWarning(string entityId, string entityName, string systemName)
		{
			if (string.IsNullOrEmpty(systemName))
				return Json(new { Result = string.Empty }, JsonRequestBehavior.AllowGet);

			int parsedEntityId;
			int.TryParse(entityId, out parsedEntityId);
			var validatedSystemName = UrlExtensions.ValidateSystemName(parsedEntityId, entityName, systemName, null, false);

			if (systemName.Equals(validatedSystemName, StringComparison.InvariantCultureIgnoreCase))
				return Json(new { Result = string.Empty }, JsonRequestBehavior.AllowGet);

			return Json(new { Result = string.Format("Entered page name already exists, so it will be replaced by '{0}'", validatedSystemName) }, JsonRequestBehavior.AllowGet);
		}

	}
}
