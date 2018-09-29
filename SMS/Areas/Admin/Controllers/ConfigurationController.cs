using System;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class ConfigurationController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		public readonly ISettingService _settingService;
		private readonly IPermissionService _permissionService;
		private readonly ITemplateService _templateService;

		#endregion Fileds

		#region Constructor

		public ConfigurationController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IPermissionService permissionService, ITemplateService templateService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._permissionService = permissionService;
			this._templateService = templateService;
		}

		#endregion

		public ActionResult Settings()
		{
			if (!_permissionService.Authorize("ManageConfiguration"))
				return AccessDeniedView();

			var model = new ConfigurationSettingsModel();
			model.ActiveSettings = "ConfigurationSettings";

			// Bind Locations
			model.AvailableLocations.Add(new SelectListItem() { Text = "Select", Value = "0", Selected = true });
			model.AvailableLocations.Add(new SelectListItem() { Text = "Top", Value = "Top" });
			model.AvailableLocations.Add(new SelectListItem() { Text = "Bottom", Value = "Bottom" });

			// Bind Templates
			model.AvailableCommentOnEventTemplates = model.AvailableCommentOnProductTemplates = model.AvailableForgotPasswordTemplates = model.AvailableNewUserRegisterTemplates = model.AvailableProductAddedTemplates = model.AvailableReplyOnCommentTemplates = model.AvailableRequestQuoteTemplates = model.AvailableUserSignInAttemptTemplates = model.AvailableVisitorQueryTemplates = _templateService.GetAllTemplates(true).Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Name.ToString()
			}).ToList();

			var itemsperpagesetting = _settingService.GetSettingByKey("ItemsPerPage", _userContext.CurrentUser.Id);

			if (itemsperpagesetting != null)
				model.ItemsPerPage = Convert.ToInt32(itemsperpagesetting.Value);

			var pagerlocation = _settingService.GetSettingByKey("PagerLocation", _userContext.CurrentUser.Id);
			if (pagerlocation != null)
				model.PagerLocation = pagerlocation.Value;

			var commentonblog = _settingService.GetSettingByKey("CommentOnBlog", _userContext.CurrentUser.Id);
			if (commentonblog != null)
				model.SelectedCommentOnBlogTemplate = commentonblog.Value;

			var forgotpasswordtemplate = _settingService.GetSettingByKey("ForgotPasswordTemplate", _userContext.CurrentUser.Id);

			if (forgotpasswordtemplate != null)
				model.SelectedEmailTemplateForForgotPassword = forgotpasswordtemplate.Value;

			var visitorqueryplacedtemplate = _settingService.GetSettingByKey("VisitorQueryPlaced", _userContext.CurrentUser.Id);

			if (visitorqueryplacedtemplate != null)
				model.SelectedVisitorQueryTemplate = visitorqueryplacedtemplate.Value;

			var commentoneventtemplate = _settingService.GetSettingByKey("CommentOnEvent", _userContext.CurrentUser.Id);

			if (commentoneventtemplate != null)
				model.CommentOnEventTemplate = commentoneventtemplate.Value;

			var commentonproducttemplate = _settingService.GetSettingByKey("CommentOnProduct", _userContext.CurrentUser.Id);

			if (commentonproducttemplate != null)
				model.CommentOnProductTemplate = commentonproducttemplate.Value;

			var productaddedtemplate = _settingService.GetSettingByKey("ProductAdded", _userContext.CurrentUser.Id);

			if (productaddedtemplate != null)
				model.ProductAddedTemplate = productaddedtemplate.Value;

			var replyoncommenttemplate = _settingService.GetSettingByKey("ReplyOnComment", _userContext.CurrentUser.Id);

			if (replyoncommenttemplate != null)
				model.ReplyOnCommentTemplate = replyoncommenttemplate.Value;

			var newuserregistertemplate = _settingService.GetSettingByKey("NewUserRegister", _userContext.CurrentUser.Id);

			if (newuserregistertemplate != null)
				model.NewUserRegisterTemplate = newuserregistertemplate.Value;

			var usersigninattempttemplate = _settingService.GetSettingByKey("UserSignInAttempt", _userContext.CurrentUser.Id);

			if (usersigninattempttemplate != null)
				model.UserSignInAttemptTemplate = usersigninattempttemplate.Value;

			var requestquotetemplate = _settingService.GetSettingByKey("RequestQuote", _userContext.CurrentUser.Id);

			if (requestquotetemplate != null)
				model.RequestQuoteTemplate = requestquotetemplate.Value;

			if (model.AvailableCommentOnEventTemplates.Count > 0)
				model.AvailableCommentOnEventTemplates.First().Selected = false;

			if (model.AvailableCommentOnProductTemplates.Count > 0)
				model.AvailableCommentOnProductTemplates.First().Selected = false;

			if (model.AvailableForgotPasswordTemplates.Count > 0)
				model.AvailableForgotPasswordTemplates.First().Selected = false;

			if (model.AvailableNewUserRegisterTemplates.Count > 0)
				model.AvailableNewUserRegisterTemplates.First().Selected = false;	 

			if (model.AvailableProductAddedTemplates.Count > 0)
				model.AvailableProductAddedTemplates.First().Selected = false;

			if (model.AvailableReplyOnCommentTemplates.Count > 0)
				model.AvailableReplyOnCommentTemplates.First().Selected = false;

			if (model.AvailableRequestQuoteTemplates.Count > 0)
				model.AvailableRequestQuoteTemplates.First().Selected = false;

			if (model.AvailableUserSignInAttemptTemplates.Count > 0)
				model.AvailableUserSignInAttemptTemplates.First().Selected = false;

			if (model.AvailableVisitorQueryTemplates.Count > 0)
				model.AvailableVisitorQueryTemplates.First().Selected = false;

			if(model.AvailableCommentOnBlogTemplates.Count > 0)
			model.AvailableCommentOnBlogTemplates.First().Selected = false;

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Settings(ConfigurationSettingsModel model)
		{
			if (!_permissionService.Authorize("ManageConfiguration"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;

			if (user != null)
			{
				if (ModelState.IsValid)
				{
					var itemsperpagesetting = _settingService.GetSettingByKey("ItemsPerPage", _userContext.CurrentUser.Id);

					if (itemsperpagesetting != null)
					{
						itemsperpagesetting.Value = model.ItemsPerPage.ToString();
						_settingService.Update(itemsperpagesetting);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.ItemsPerPage.ToString();
						setting.TypeId = 4;
						setting.Name = "ItemsPerPage";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var commentonblog = _settingService.GetSettingByKey("CommentOnBlog", _userContext.CurrentUser.Id);
					if (commentonblog != null)
					{
						commentonblog.Value = model.SelectedCommentOnBlogTemplate.ToString();
						_settingService.Update(commentonblog);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.SelectedCommentOnBlogTemplate.ToString();
						setting.TypeId = 4;
						setting.Name = "CommentOnBlog";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var pagerlocation = _settingService.GetSettingByKey("PagerLocation", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						pagerlocation.Value = model.PagerLocation;
						_settingService.Update(pagerlocation);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.PagerLocation.ToString();
						setting.TypeId = 4;
						setting.Name = "PagerLocation";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var forgotpasswordtemplate = _settingService.GetSettingByKey("ForgotPasswordTemplate", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						forgotpasswordtemplate.Value = model.SelectedEmailTemplateForForgotPassword;
						_settingService.Update(forgotpasswordtemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.SelectedEmailTemplateForForgotPassword.ToString();
						setting.TypeId = 4;
						setting.Name = "ForgotPasswordTemplate";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var visitorqueryplacedtemplate = _settingService.GetSettingByKey("VisitorQueryPlaced", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						visitorqueryplacedtemplate.Value = model.SelectedVisitorQueryTemplate;
						_settingService.Update(visitorqueryplacedtemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.SelectedVisitorQueryTemplate;
						setting.TypeId = 4;
						setting.Name = "VisitorQueryPlaced";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var commentoneventtemplate = _settingService.GetSettingByKey("CommentOnEvent", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						commentoneventtemplate.Value = model.CommentOnEventTemplate;
						_settingService.Update(commentoneventtemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.CommentOnEventTemplate;
						setting.TypeId = 4;
						setting.Name = "CommentOnEvent";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var commentonproducttemplate = _settingService.GetSettingByKey("CommentOnProduct", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						commentonproducttemplate.Value = model.CommentOnProductTemplate;
						_settingService.Update(commentonproducttemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.CommentOnProductTemplate;
						setting.TypeId = 4;
						setting.Name = "CommentOnProduct";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var productaddedtemplate = _settingService.GetSettingByKey("ProductAdded", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						productaddedtemplate.Value = model.ProductAddedTemplate;
						_settingService.Update(productaddedtemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.ProductAddedTemplate;
						setting.TypeId = 4;
						setting.Name = "ProductAdded";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var replyoncommenttemplate = _settingService.GetSettingByKey("ReplyOnComment", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						replyoncommenttemplate.Value = model.ReplyOnCommentTemplate;
						_settingService.Update(replyoncommenttemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.ReplyOnCommentTemplate;
						setting.TypeId = 4;
						setting.Name = "ReplyOnComment";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var newuserregistertemplate = _settingService.GetSettingByKey("NewUserRegister", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						newuserregistertemplate.Value = model.NewUserRegisterTemplate;
						_settingService.Update(newuserregistertemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.NewUserRegisterTemplate;
						setting.TypeId = 4;
						setting.Name = "NewUserRegister";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var usersigninattempttemplate = _settingService.GetSettingByKey("UserSignInAttempt", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						usersigninattempttemplate.Value = model.UserSignInAttemptTemplate;
						_settingService.Update(usersigninattempttemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.UserSignInAttemptTemplate;
						setting.TypeId = 4;
						setting.Name = "UserSignInAttempt";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}

					var requestquotetemplate = _settingService.GetSettingByKey("RequestQuote", _userContext.CurrentUser.Id);
					if (pagerlocation != null)
					{
						requestquotetemplate.Value = model.RequestQuoteTemplate;
						_settingService.Update(requestquotetemplate);
					}
					else
					{
						// Insert
						var setting = new Settings();
						setting.UserId = _userContext.CurrentUser.Id;
						setting.SettingType = 4;
						setting.Value = model.RequestQuoteTemplate;
						setting.TypeId = 4;
						setting.Name = "RequestQuote";
						setting.CreatedOn = DateTime.Now;
						setting.ModifiedOn = DateTime.Now;
						setting.Entity = "Configuration";
						setting.EntityId = 1;
						_settingService.Insert(setting);
					}
				}
			}

			SuccessNotification("Configuration Settings Saved Successfully");
			model.ActiveSettings = "ConfigurationSettings";

			return RedirectToAction("Settings");
		}

	}
}
