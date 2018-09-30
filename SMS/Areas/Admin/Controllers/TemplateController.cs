using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class TemplateController : AdminAreaController
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

		#endregion Fileds

		#region Constructor

		public TemplateController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IProductService productService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IFileService fileService, ITemplateService templateService, ICustomPageService customPageService, IPermissionService permissionService)
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
		}

		#endregion

		public ActionResult TemplateList()
		{
			var model = new List<TemplateModel>();
			if (!_permissionService.Authorize("ManageTemplates"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			if (user != null)
			{
				var lstTemplates = _templateService.GetAllTemplates().Where(x => x.UserId == user.Id).OrderByDescending(x => x.CreatedOn).ToList();
				if (lstTemplates.Count > 0)
				{
					foreach (var temp in lstTemplates)
					{
						model.Add(new TemplateModel
						{
							BodyHtml = temp.BodyHtml,
							IsActive = temp.IsActive,
							TemplateId = temp.Id,
							IsSystemDefined = temp.IsSystemDefined,
							Id = temp.Id,
							Name = temp.Name,
							UserId = temp.UserId,
							dataTokens = temp.Tokens.Select(dt => new DataTokenModel()
							{
								Id = dt.Id,
								IsSystemDefined = dt.IsSystemDefined,
								Name = dt.Name,
								UserId = dt.UserId,
								Value = dt.Value
							}).ToList()
						});
					}
				}
			}

			return View(model);
		}

		public ActionResult Edit(int templateId)
		{
			var model = new CreateTemplateModel();
			if (!_permissionService.Authorize("ManageTemplates"))
				return AccessDeniedView();

			if (templateId == 0)
				throw new Exception("Template Id Missing");

			var temp = _templateService.GetTemplateById(templateId);
			// Get All Data Tokens
			var dtTokens = _templateService.GetAllDataTokens(true).ToList();
			var user = _userContext.CurrentUser;
			if (user != null)
			{
				if (temp != null)
				{
					if (temp.UserId == user.Id)
					{
						model = new CreateTemplateModel
						{
							InsertDataTokensModel = dtTokens.Select(x => new CreateTemplateModel.CreateDataTokensModel()
							{
								Id = x.Id,
								UserId = x.UserId,
								IsSystemDefined = x.IsSystemDefined,
								Name = x.Name,
								Value = x.Value,
								IsActive = x.IsActive
							}).ToList(),
							Name = temp.Name,
							IsActive = temp.IsActive,
							BodyHtml = temp.BodyHtml,
							Id = temp.Id,
							UserId = temp.UserId,
							IsSystemDefined = temp.IsSystemDefined
						};
					}
				}
			}

			if (model.InsertDataTokensModel == null)
				model.InsertDataTokensModel = new List<CreateTemplateModel.CreateDataTokensModel>();

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(CreateTemplateModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageTemplates"))
				return AccessDeniedView();

			// Check for duplicate templates, if any
			var _template = _templateService.GetTemplateByName(model.Name);
			if (_template != null && _template.Id != model.Id)
				ModelState.AddModelError("Name", "A Template with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var temp = _templateService.GetTemplateById(model.Id);
				if (temp != null)
				{
					if (frm.Keys.Count > 0)
					{
						foreach (var k in frm.AllKeys.Where(x => x.Contains("token_")))
						{
							var dtTokenId = Convert.ToInt32(frm[k].ToString());
							if (temp.Tokens.FirstOrDefault(x => x.Id == dtTokenId) == null)
							{
								var tkn = _templateService.GetDataTokenById(dtTokenId);
								if (tkn != null)
								{
									var dtToken = new DataToken()
									{
										Id = tkn.Id,
										IsSystemDefined = tkn.IsSystemDefined,
										Name = tkn.Name,
										UserId = tkn.UserId,
										Value = tkn.Value,
										CreatedOn = DateTime.Now,
										ModifiedOn = DateTime.Now,
										IsActive = tkn.IsActive,
										IsDeleted = false
									};
									temp.Tokens.Add(dtToken);
								}
							}
						}
					}

					var alltokens = _templateService.GetAllDataTokensByTemplate(temp.Id);

					if (alltokens.Count > 0)
					{
						foreach (DataToken dToken in alltokens)
						{
							if (!model.BodyHtml.Contains("[" + dToken.Name + "]"))
							{
								temp.Tokens.Remove(dToken);
							}
						}
					}

					temp.CreatedOn = DateTime.Now;
					temp.Name = model.Name;
					temp.BodyHtml = model.BodyHtml;
					temp.IsActive = model.IsActive;
					temp.Id = model.Id;
					temp.Url = "";
					temp.ModifiedOn = DateTime.Now;
					temp.IsSystemDefined = model.IsSystemDefined;
					_templateService.Update(temp);

					
				}
			}

			SuccessNotification("Template updated successfully.");
			return RedirectToAction("TemplateList");
		}
		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageTemplates"))
				return AccessDeniedView();

			var model = new CreateTemplateModel();
			// Get All Data Tokens
			var dtTokens = _templateService.GetAllDataTokens(true).ToList();
			model.InsertDataTokensModel = dtTokens.Select(x => new CreateTemplateModel.CreateDataTokensModel()
			{
				Id = x.Id,
				UserId = x.UserId,
				IsSystemDefined = x.IsSystemDefined,
				Name = x.Name,
				Value = x.Value,
				IsActive = x.IsActive
			}).ToList();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(CreateTemplateModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageTemplates"))
				return AccessDeniedView();

			// Check for duplicate templates, if any
			var _template = _templateService.GetTemplateByName(model.Name);
			if (_template != null)
				ModelState.AddModelError("Name", "A Template with the same name already exists. Please choose a different name.");
			var user = _userContext.CurrentUser;
			if (user != null)
			{

				if (ModelState.IsValid)
				{
					var template = new Template
					{
						BodyHtml = model.BodyHtml,
						IsActive = model.IsActive,
						Name = model.Name,
						UserId = user.Id,
						IsDeleted = false,
						CreatedOn = DateTime.Now,
						ModifiedOn = DateTime.Now,
						Url = "",
						IsSystemDefined = false
					};

					var dtTokens = _templateService.GetAllDataTokens(true).ToList();
					if (dtTokens.Count > 0)
					{
						foreach (DataToken dToken in dtTokens)
						{
							if (template.BodyHtml.Contains("[" + dToken.Name + "]"))
							{
								var dtToken = new DataToken()
								{
									Id = dToken.Id,
									IsSystemDefined = dToken.IsSystemDefined,
									Name = dToken.Name,
									UserId = dToken.UserId,
									Value = dToken.Value,
									CreatedOn = DateTime.Now,
									ModifiedOn = DateTime.Now,
									IsActive = dToken.IsActive,
									IsDeleted = false
								};
								template.Tokens.Add(dtToken);
							}
						}
					}

					_templateService.Insert(template);
				}
				else
				{
					return View(model);
				}
			}

			SuccessNotification("Template created successfully.");
			return RedirectToAction("TemplateList");
		}
		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageTemplates"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			var _isSystemDefine = _templateService.GetTemplateById(id).IsSystemDefined;

			if (!_isSystemDefine)
				_templateService.DeleteTemplate(id);

			SuccessNotification("Template deleted successfully.");
			return RedirectToAction("TemplateList");
		}

	}
}
