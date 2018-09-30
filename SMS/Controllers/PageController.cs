using System;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Controllers
{
	public partial class PageController : PublicHttpController
	{
		#region Fields

		private readonly IUrlHelper _urlHelper;
		private readonly ICustomPageService _customPageService;
		private readonly IUserContext _userContext;
		private readonly ITemplateService _templateService;
		private readonly IPermissionService _permissionService;

		#endregion

		#region Constructors

		public PageController(IUrlHelper urlHelper,
			 IUserContext userContext,
			 ITemplateService templateService,
			 IPermissionService permissionService,
			 ICustomPageService customPageService)
		{
			this._urlHelper = urlHelper;
			this._userContext = userContext;
			this._templateService = templateService;
			this._permissionService = permissionService;
			this._customPageService = customPageService;
		}

		#endregion

		#region Utilities

		[NonAction]
		protected virtual CustomPageModel PreparePageModel(CustomPage page)
		{
			if (page == null)
				throw new ArgumentNullException("custompage");

			var model = new CustomPageModel
			{
				Id = page.Id,
				SystemName = page.GetSystemName(),
				Name = page.Name,
				BodyHtml = page.Template.BodyHtml,
				MetaKeywords = page.MetaKeywords,
				MetaDescription = page.MetaDescription,
				MetaTitle = page.MetaTitle,
				TemplateId = page.TemplateId,
				IncludeInTopMenu = page.IncludeInTopMenu,
				IncludeInFooterColumn1 = page.IncludeInFooterColumn1,
				IncludeInFooterColumn2 = page.IncludeInFooterColumn2,
				IncludeInFooterColumn3 = page.IncludeInFooterColumn3,
				IncludeInFooterMenu = page.IncludeInFooterMenu,
				IsSystemDefined = page.IsSystemDefined,
				IsActive = page.IsActive,
				PermissionOriented = page.PermissionOriented
			};
			return model;
		}

		#endregion

		#region Methods

		public ActionResult PageDetails(int custompageid)
		{
			var page = _customPageService.GetCustomPageById(custompageid);
			if (page == null)
				return null;
			if (!page.IsActive)
				return null;

			var model = new CustomPageModel();
			model = PreparePageModel(page);

			if (model == null)
				return RedirectToRoute("Root");

			//template
			var template = _templateService.GetTemplateById(model.TemplateId);
			if (template == null)
				template = _templateService.GetAllTemplates(true).FirstOrDefault();
			if (template == null)
				throw new Exception("No default template could be loaded");

			return View(model);
		}

		[ChildActionOnly]
		public ActionResult Page(string systemName)
		{
			if (systemName == null)
				return Content("");

			var page = _customPageService.GetCustomPageBySystemName(systemName);
			if (page == null)
				return null;

			if (!page.IsActive)
				return null;

			var model = page.ToModel();

			if (model == null)
				return Content("");

			return PartialView(model);
		}

		#endregion
	}
}
