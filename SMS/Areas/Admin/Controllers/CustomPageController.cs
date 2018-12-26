using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class CustomPageController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IProductService _productService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;
		private readonly IFileService _fileService;
		private readonly ITemplateService _templateService;
		private readonly ICustomPageService _customPageService;
		private readonly IPermissionService _permissionService;
		private readonly IUrlHelper _urlHelper;
		private readonly IUrlService _urlService;

		#endregion Fileds

		#region Constructor

		public CustomPageController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IProductService productService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IFileService fileService, ITemplateService templateService, ICustomPageService customPageService, IPermissionService permissionService, IUrlHelper urlHelper, IUrlService urlService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._productService = productService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
			this._fileService = fileService;
			this._templateService = templateService;
			this._customPageService = customPageService;
			this._permissionService = permissionService;
			this._urlHelper = urlHelper;
			this._urlService = urlService;
		}

        #endregion

        #region Utilities

	    public ActionResult LoadGrid()
	    {
	        try
	        {
	            var draw = Request.Form.GetValues("draw").FirstOrDefault();
	            var start = Request.Form.GetValues("start").FirstOrDefault();
	            var length = Request.Form.GetValues("length").FirstOrDefault();
	            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() + "][name]")?.FirstOrDefault();
	            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();
	            var searchValue = Request.Form.GetValues("search[value]")?.FirstOrDefault();


	            //Paging Size (10,20,50,100)    
	            int pageSize = length != null ? Convert.ToInt32(length) : 0;
	            int skip = start != null ? Convert.ToInt32(start) : 0;
	            int recordsTotal = 0;

	            // Getting all data    
	            var pagesData = (from temppages in _customPageService.GetAllCustomPages(showSystemDefined: false) select temppages);

	            //Search    
	            if (!string.IsNullOrEmpty(searchValue))
	            {
	                pagesData = pagesData.Where(m => m.Name.Contains(searchValue) || m.SystemName.Contains(searchValue));
	            }

	            //total number of rows count     
	            var customPages = pagesData as CustomPage[] ?? pagesData.ToArray();
	            recordsTotal = customPages.Count();
	            //Paging     
	            var data = customPages.Skip(skip).Take(pageSize);

	            //Returning Json Data 
	            return new JsonResult()
	            {
	                Data = new
	                {
	                    draw = draw,
	                    recordsFiltered = recordsTotal,
	                    recordsTotal = recordsTotal,
	                    data = data.Select(x => x.ToModel()).OrderBy(x => x.Name).ToList()
	                },
	                ContentEncoding = Encoding.Default,
	                ContentType = "application/json",
	                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
	                MaxJsonLength = int.MaxValue
	            };
	        }
	        catch (Exception ex)
	        {
	            throw new Exception(ex.Message);
	        }

	    }

        #endregion

        public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageCustomPages"))
				return AccessDeniedView();

			var model = new List<CustomPageModel>();
			var user = _userContext.CurrentUser;
			var lstCustomPages = _customPageService.GetAllCustomPages().Where(x => x.UserId == user.Id).OrderByDescending(x => x.CreatedOn).ToList();
			if (lstCustomPages.Count > 0)
			{
				foreach (var customPage in lstCustomPages)
				{
					var customPageModel = customPage.ToModel();
					var templateModel = customPage.Template?.ToModel();

					if (customPage.Template != null)
					{
						foreach (var dt in customPage.Template.Tokens)
						{
							var dtModel = dt.ToModel();
							templateModel.dataTokens.Add(dtModel);
						}
						customPageModel.template = templateModel;
					}
					model.Add(customPageModel);
				}
			}

			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageCustomPages"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("custom Page Id Missing");

			var model = new CustomPageModel();
			var custompage = _customPageService.GetCustomPageById(id);

			if (custompage != null)
			{
				model = custompage.ToModel();
				model.Url = Url.RouteUrl("CustomPage", new { name = custompage.GetSystemName() }, "http");
				var templateModel = custompage.Template?.ToModel();

				if (custompage.Template != null)
				{
					foreach (var dt in custompage.Template.Tokens)
					{
						var dtModel = dt.ToModel();
						templateModel.dataTokens.Add(dtModel);
					}
					model.template = templateModel;
				}

				// Load Available Templates
				model.AvailableTemplates.Add(new SelectListItem { Text = "-- Select Template --", Value = "0" });
				foreach (var t in _templateService.GetAllTemplates())
					model.AvailableTemplates.Add(new SelectListItem { Text = t.Name, Value = t.Id.ToString() });

				if (model.TemplateId > 0)
				{
					foreach (var item in model.AvailableTemplates.Where(ss => ss.Value == model.TemplateId.ToString()))
						item.Selected = true;

					model.AvailableTemplates.First().Selected = false;
				}
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(CustomPageModel model)
		{
			if (!_permissionService.Authorize("ManageCustomPages"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;

			// Check for duplicate pages, if any
			var _page = _customPageService.GetCustomPageByName(model.Name);
			if (_page != null && _page.Id != model.Id)
				ModelState.AddModelError("Name", "A Custom Page with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var custom = _customPageService.GetCustomPageById(model.Id);
				if (custom != null)
				{
					custom.Name = model.Name;
					custom.TemplateId = model.TemplateId;
					custom.Template = _templateService.GetTemplateById(model.TemplateId);

					if (!custom.IsSystemDefined)
					{
						custom.IsActive = model.IsActive;
						custom.IsSystemDefined = model.IsSystemDefined;
					}

					custom.ModifiedOn = DateTime.Now;
					custom.Url = Url.RouteUrl("CustomPage", new { name = custom.GetSystemName() });
					custom.SystemName = custom.GetSystemName();
					custom.UserId = user.Id;
					custom.PermissionOriented = model.PermissionOriented;
					custom.PermissionRecordId = model.PermissionRecordId;
					custom.DisplayOrder = model.DisplayOrder;
					custom.IncludeInFooterColumn1 = model.IncludeInFooterColumn1;
					custom.IncludeInFooterColumn2 = model.IncludeInFooterColumn2;
					custom.IncludeInFooterColumn3 = model.IncludeInFooterColumn3;
					custom.IncludeInTopMenu = model.IncludeInTopMenu;
					custom.IncludeInFooterMenu = model.IncludeInFooterMenu;
					custom.MetaDescription = model.MetaDescription;
					custom.MetaKeywords = model.MetaKeywords;
					custom.MetaTitle = model.MetaTitle;
					_customPageService.Update(custom);

					if (!custom.IsSystemDefined)
					{
						model.SystemName = custom.ValidateSystemName(model.SystemName, model.Name, true);
						_urlService.SaveSlug(custom, model.SystemName);
					}

				}
			}
			else
			{
				model.AvailableTemplates.Add(new SelectListItem { Text = "-- Select Template --", Value = "0" });
				foreach (var t in _templateService.GetAllTemplates())
					model.AvailableTemplates.Add(new SelectListItem { Text = t.Name, Value = t.Id.ToString() });

				return View(model);
			}

			SuccessNotification("Page saved successfully.");
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageCustomPages"))
				return AccessDeniedView();

			var model = new CustomPageModel();
			// Load Available Templates
			model.AvailableTemplates.Add(new SelectListItem { Text = "-- Select Template --", Value = "0" });
			foreach (var t in _templateService.GetAllTemplates())
				model.AvailableTemplates.Add(new SelectListItem { Text = t.Name, Value = t.Id.ToString() });

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(CustomPageModel model)
		{
			if (!_permissionService.Authorize("ManageCustomPages"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			// Check for duplicate pages, if any
			var page = _customPageService.GetCustomPageByName(model.Name);
			if (page != null)
				ModelState.AddModelError("Name", "A Custom Page with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var custom = new CustomPage();
				custom.Name = model.Name;
				custom.TemplateId = model.TemplateId;
				custom.Template = _templateService.GetTemplateById(model.TemplateId);
				custom.IsActive = model.IsActive;
				custom.ModifiedOn = DateTime.Now;
				custom.CreatedOn = DateTime.Now;
				custom.Url = model.Url;
				custom.UserId = user.Id;
				custom.IsSystemDefined = false;
				custom.PermissionOriented = model.PermissionOriented;
				custom.PermissionRecordId = model.PermissionRecordId;
				custom.DisplayOrder = model.DisplayOrder;
				custom.IncludeInFooterColumn1 = model.IncludeInFooterColumn1;
				custom.IncludeInFooterColumn2 = model.IncludeInFooterColumn2;
				custom.IncludeInFooterColumn3 = model.IncludeInFooterColumn3;
				custom.IncludeInTopMenu = model.IncludeInTopMenu;
				custom.IncludeInFooterMenu = model.IncludeInFooterMenu;
				custom.MetaDescription = model.MetaDescription;
				custom.MetaKeywords = model.MetaKeywords;
				custom.MetaTitle = model.MetaTitle;
				custom.PermissionOriented = false;
				custom.PermissionRecordId = _permissionService.GetAllPermissions().FirstOrDefault() != null
					// ReSharper disable once PossibleNullReferenceException
					? _permissionService.GetAllPermissions().FirstOrDefault().Id
					: 0;
				custom.PermissionRecord = _permissionService.GetAllPermissions().FirstOrDefault();
				_customPageService.Insert(custom);

				// Save URL Record
				model.SystemName = custom.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(custom, model.SystemName);
			}
			else
			{
				// Load Available Templates
				model.AvailableTemplates.Add(new SelectListItem { Text = "-- Select Template --", Value = "0" });
				foreach (var t in _templateService.GetAllTemplates())
					model.AvailableTemplates.Add(new SelectListItem { Text = t.Name, Value = t.Id.ToString() });

				return View(model);
			}

			SuccessNotification("Page created successfully.");
			return RedirectToAction("List");
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageCustomPages"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			var _isSystemDefine = _customPageService.GetCustomPageById(id)?.IsSystemDefined;

			if (_isSystemDefine != null)
				if (!_isSystemDefine.Value)
					_customPageService.Delete(id);

			SuccessNotification("Page deleted successfully.");
			return RedirectToAction("List");
		}

	}
}
