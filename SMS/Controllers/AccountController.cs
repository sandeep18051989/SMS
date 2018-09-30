using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class AccountController : PublicHttpController
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly IUserService _userService;
		private readonly IUserContext _userContext;
		private readonly ISettingService _settingService;
		private readonly ITemplateService _templateService;
		private readonly IEmailService _emailService;
		private readonly IAuditService _auditService;
		private readonly IEventService _eventService;
		private readonly ICommentService _commentService;
		private readonly IProductService _productService;
		private readonly IBlogService _blogService;
		private readonly IUrlHelper _urlHelper;

		public AccountController(IAuthenticationService authenticationService, IUserService userService, IUserContext userContext, ISettingService settingService, ITemplateService templateService, IEmailService emailService, IAuditService auditService, IEventService eventService, ICommentService commentService, IProductService productService, IBlogService blogService, IUrlHelper urlHelper)
		{
			this._authenticationService = authenticationService;
			this._userService = userService;
			this._userContext = userContext;
			this._settingService = settingService;
			this._templateService = templateService;
			this._emailService = emailService;
			this._auditService = auditService;
			this._eventService = eventService;
			this._commentService = commentService;
			this._productService = productService;
			this._blogService = blogService;
			this._urlHelper = urlHelper;
		}

		[AllowAnonymous]
		public ActionResult Register()
		{
			var model = new RegisterModel();
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			// Get Acedmic Year
			var acedmicYear = _settingService.GetSettingByKey("acedemicyear");
			if (acedmicYear == null)
			{
				ErrorNotification("Acedmic Year is missing.");
				return View(model);
			}

			if (ModelState.IsValid)
			{
				var newUser = new User()
				{
					CreatedOn = DateTime.Now,
					IsActive = true,
					IsApproved = false,
					IsDeleted = false,
					ModifiedOn = DateTime.Now,
					Password = model.Password,
					UserName = model.Email,
					UserGuid = Guid.NewGuid(),
					UserId = 1
				};
				_userService.Insert(newUser);
				model.SystemName = newUser.UserName.Trim().Replace(" ", "");

				// Insert UserInfo
				//var _userInfo = new UserInfo()
				//{
				//	FirstName = model.FirstName,
				//	LastName = model.LastName,
				//	CreatedOn = DateTime.Now,
				//	ModifiedOn = DateTime.Now,
				//	AddressLine1 = "",
				//	AddressLine2 = "",
				//	BriefIntroduction = "",
				//	CityId = 0,
				//	CoverPictureId = 0,
				//	Email = model.Email,
				//	IsEmailVerified = false,
				//	IsPhoneVerified = false,
				//	Hobbies = "",
				//	Phone = "",
				//	ProfilePictureId = 0,
				//	Street = "",
				//	UserId = newUser.Id,
				//	AcadmicYear = acedmicYear.Value,
				//	FacebookLink = "",
				//	FreelancerLink = "",
				//	GooglePlusLink = "",
				//	GuruLink = "",
				//	Hi5Link = "",
				//	InstagramLink = "",
				//	LinkedInLink = "",
				//	PInterestLink = "",
				//	TweeterLink = "",
				//	UpworkLink = ""
				//};

				//_userInfoService.Insert(_userInfo);

				// Save URL Record
				//model.SystemName = _userInfo.ValidateSystemName(model.SystemName, model.FirstName, true);
				//_urlHelper.SaveSlug(_userInfo, model.SystemName);

				// Get Email Settings for Use
				var _settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);

				// Send Notification To The Admin
				if (_settings.Count > 0)
				{
					var template = _settingService.GetSettingByKey("NewUserRegister");
					var Template = _templateService.GetTemplateByName(template.Value);
					if (Template != null)
					{
						var tokens = new List<DataToken>();
						_templateService.AddUserTokens(tokens, newUser);

						foreach (var dt in tokens)
						{
							Template.BodyHtml = EF.Core.CodeHelper.Replace(Template.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
						}

						var setting = _settingService.GetSettingByKey("FromEmail");
						if (!String.IsNullOrEmpty(setting.Value))
							_emailService.SendMailUsingTemplate(setting.Value, newUser.UserName + "- Just Registered", Template);
					}
				}

				SuccessNotification("Your registration is successful, But needs approval from the admin. We will get back to you shortly.");
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}

			return View(model);
		}

		[AllowAnonymous]
		public ActionResult Login(string ReturnUrl = null)
		{
			var model = new LoginModel();
			if (!String.IsNullOrEmpty(ReturnUrl))
				model.ReturnUrl = ReturnUrl;

			return View(model);
		}

		public ActionResult Developer()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken()]
		public ActionResult Login(LoginModel model)
		{
			ViewData["ReturnUrl"] = model.ReturnUrl;
			if (ModelState.IsValid)
			{
				var _user = _userService.GetUserByUsername(model.Email);
				if (_user != null && _user.IsApproved)
				{
					if (_user.Password == model.Password)
					{
						//var userinformation = _userInfoService.GetUserInformationByUserId(_user.Id);

						// Update Last Login Date
						_user.LastLoginDate = DateTime.Now;
						_userService.Update(_user);

						//sign in customer
						_authenticationService.SignIn(_user, model.RememberMe);

						// Send Notification To The User
						var template = _settingService.GetSettingByKey("UserSignInAttempt");
						if (template != null)
						{
							var Template = _templateService.GetTemplateByName(template.Value);

							var tokens = new List<DataToken>();
							_templateService.AddUserTokens(tokens, _user);

							foreach (var dt in tokens)
							{
								Template.BodyHtml = EF.Core.CodeHelper.Replace(Template.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
							}

							//var _userInfo = _userInfoService.GetUserInformationByUserId(_user.Id);
							//if (_userInfo != null)
							//{
							//	if (String.IsNullOrEmpty(_userInfo.Email))
							//	{
							//		SuccessNotification("You haven't updated your eamil address with us. Update your profile <a href='" + Url.Action("EditMyAccount", "Account", new { @id = _user.Id }) + "' title='My Account' >here</a>");
							//	}
							//	else
							//	{
							//		_emailService.SendMailUsingTemplate(_userInfo.Email, "Urgent : Login Notification", Template);
							//	}
							//}
						}

						_userContext.CurrentUser = _user;

						if (ViewData["ReturnUrl"] == null)
						{
							return RedirectToAction("Index", "Home");
						}
						else
						{
							return Redirect(ViewData["ReturnUrl"].ToString());
						}
					}
					else
					{
						ModelState.AddModelError("Result", "Username and Password Does Not Match. Please try Again...");
					}
				}
				else
				{
					ModelState.AddModelError("Result", "Username Does Not Exist. Please try Again...");
				}
			}

			return View(model);
		}

		public ActionResult LogOff()
		{
			_authenticationService.SignOut();

			return RedirectToAction("Index", "Home");
		}

		private ActionResult RedirectToReturnUrl(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ForgotPassword(ForgotPasswordModel model)
		{
			var _User = _userService.GetUserByUsername(model.name);
			if (_User != null)
			{
				//var _userInfo = _userInfoService.GetUserInformationByUserId(_User.Id);
				//if (_userInfo != null)
				//{
				//	if (!String.IsNullOrEmpty(_userInfo.Email))
				//	{
				//		// Get Forgot Password Template
				//		var configSetting = _settingService.GetSettingsByType(SettingTypeEnum.ConfigurationSetting);
				//		if (configSetting.Count > 0)
				//		{
				//			var templateName = configSetting.FirstOrDefault(x => x.Name.Trim().ToLower() == "forgotpasswordtemplate").Value;
				//			if (!String.IsNullOrEmpty(templateName))
				//			{
				//				var _templateContent = _templateService.GetTemplateByName(templateName);
				//				if (_templateContent != null)
				//				{
				//					var _lstTokens = new List<DataToken>();
				//					_templateService.AddUserTokens(_lstTokens, _User);

				//					if (_lstTokens.Count > 0)
				//					{
				//						foreach (DataToken userdt in _lstTokens)
				//						{
				//							_templateContent.BodyHtml = EF.Core.CodeHelper.Replace(_templateContent.BodyHtml.ToString(), "[" + userdt.Name + "]", userdt.Value, StringComparison.InvariantCulture);
				//						}
				//					}

				//					foreach (var dt in _templateService.GetAllDataTokensByTemplate(_templateContent.Id).Where(x => x.IsActive).ToList())
				//					{
				//						_templateContent.BodyHtml = EF.Core.CodeHelper.Replace(_templateContent.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
				//					}

				//					_emailService.SendMail(_userInfo.Email, "Artery Labs:Password Request", _templateContent != null ? _templateContent.BodyHtml : "Username does not exist or You haven't an updated email address. Please write to us using complaint link.");
				//					SuccessNotification("A mail has been sent to your account email address with your password.");
				//				}
				//			}
				//		}
				//		else
				//		{
				//			ErrorNotification("Configuration Template setting not available, contact administratorSS.");
				//		}
				//	}
				//	else
				//	{
				//		ErrorNotification("Username does not exist or You haven't an updated email address. Please write to us using complaint link.");
				//	}
				//}
				//else
				//{
				//	ErrorNotification("Username does not exist or You haven't an updated email address. Please write to us using complaint link.");
				//}

			}

			return RedirectToAction("Login");
		}

		#region User Information

		//public ActionResult MyAccount()
		//{
		//	if (_userContext.CurrentUser == null)
		//		return LogOff();

		//	var model = new UserInfoModel();
		//	var _userinfo = _userInfoService.GetUserInformationByUserId(_userContext.CurrentUser.Id);

		//	if (_userinfo != null)
		//	{
		//		model.AddressLine1 = !String.IsNullOrEmpty(_userinfo.AddressLine1) ? _userinfo.AddressLine1 : "";
		//		model.AddressLine2 = !String.IsNullOrEmpty(_userinfo.AddressLine2) ? _userinfo.AddressLine2 : "";
		//		model.BriefIntroduction = !String.IsNullOrEmpty(_userinfo.BriefIntroduction) ? _userinfo.BriefIntroduction : "";
		//		model.CityId = _userinfo.CityId;
		//		model.CoverPictureId = _userinfo.CoverPictureId;
		//		model.Email = !String.IsNullOrEmpty(_userinfo.Email) ? _userinfo.Email.Trim().ToLower() : "";
		//		model.FirstName = !String.IsNullOrEmpty(_userinfo.FirstName) ? _userinfo.FirstName : "";
		//		model.Hobbies = !String.IsNullOrEmpty(_userinfo.Hobbies) ? _userinfo.Hobbies : "";
		//		model.IsEmailVerified = _userinfo.IsEmailVerified;
		//		model.IsPhoneVerified = _userinfo.IsPhoneVerified;
		//		model.LastName = !String.IsNullOrEmpty(_userinfo.LastName) ? _userinfo.LastName : "";
		//		model.Phone = !String.IsNullOrEmpty(_userinfo.Phone) ? _userinfo.Phone : "";
		//		model.ProfilePictureId = _userinfo.ProfilePictureId;
		//		model.Street = !String.IsNullOrEmpty(_userinfo.Street) ? _userinfo.Street : "";
		//		model.Id = _userinfo.Id;
		//		model.FacebookLink = !String.IsNullOrEmpty(_userinfo.FacebookLink) ? _userinfo.FacebookLink : "";
		//		model.FreelancerLink = !String.IsNullOrEmpty(_userinfo.FreelancerLink) ? _userinfo.FreelancerLink : "";
		//		model.GooglePlusLink = !String.IsNullOrEmpty(_userinfo.GooglePlusLink) ? _userinfo.GooglePlusLink : "";
		//		model.GuruLink = !String.IsNullOrEmpty(_userinfo.GuruLink) ? _userinfo.GuruLink : "";
		//		model.Hi5Link = !String.IsNullOrEmpty(_userinfo.Hi5Link) ? _userinfo.Hi5Link : "";
		//		model.InstagramLink = !String.IsNullOrEmpty(_userinfo.InstagramLink) ? _userinfo.InstagramLink : "";
		//		model.LinkedInLink = !String.IsNullOrEmpty(_userinfo.LinkedInLink) ? _userinfo.LinkedInLink : "";
		//		model.PInterestLink = !String.IsNullOrEmpty(_userinfo.PInterestLink) ? _userinfo.PInterestLink : "";
		//		model.TweeterLink = !String.IsNullOrEmpty(_userinfo.TweeterLink) ? _userinfo.TweeterLink : "";
		//		model.UpworkLink = !String.IsNullOrEmpty(_userinfo.UpworkLink) ? _userinfo.UpworkLink : "";

		//		// Get User Activities
		//		model.Activities = _auditService.GetAllAuditsByUser(_userinfo.UserId.ToString()).AsEnumerable().Select(x => new AuditModel()
		//		{
		//			AuditLogId = x.AuditLogId,
		//			EventDateUTC = x.EventDateUTC,
		//			EventType = x.EventType,
		//			LogDetails = x.LogDetails.GroupBy(d => d.AuditLogId).Select(d => d.First()).Take(10).ToList(),
		//			Metadata = x.Metadata.ToList(),
		//			RecordId = x.RecordId,
		//			TypeFullName = x.TypeFullName,
		//			EntityName = GetEntityName(x),
		//		}).ToList();

		//		model.Activities = model.Activities.GroupBy(x => x.EntityName).Select(x => x.FirstOrDefault()).Take(10).ToList();

		//		// Get User Events
		//		model.EventsUploadedList = _eventService.GetAllEventsByUser(_userinfo.UserId).AsEnumerable().OrderByDescending(x => x.CreatedOn).Take(10).ToList();

		//		// Get User Comments
		//		model.CommentsList = _commentService.GetCommentsByUser(_userinfo.UserId).Take(10).ToList();

		//		// Get User Blogs
		//		model.BlogsUploadedList = _blogService.GetBlogsByUser(_userinfo.UserId).Take(10).ToList();

		//		// Get User Products
		//		model.ProductsUploadedList = _productService.GetAllProductByUser(_userinfo.UserId).Take(5).ToList();
		//		model.user = _userContext.CurrentUser;
		//	}

		//	return View(model);
		//}

		public JsonResult CheckEmailExists(string email)
		{
			if (String.IsNullOrEmpty(email))
				throw new Exception("Email parameter is missing.");

			var _userData = _userService.GetUserByUsername(email);

			if (_userData != null)
				return Json(true, JsonRequestBehavior.AllowGet);

			return Json(false, JsonRequestBehavior.AllowGet);
		}

		public ActionResult EditMyAccount(int id)
		{
			if (id == 0 || _userContext.CurrentUser == null)
				return RedirectToAction("Login");

			if (_userContext.CurrentUser.Id != id && _userContext.CurrentUser.Roles.Any(x => x.Id != 1))
				return LogOff();

			var model = new UserInfoModel();
			//var _userinfo = _userInfoService.GetUserInformationByUserId(id);

			//if (_userinfo != null)
			//{
			//	model.AddressLine1 = _userinfo.AddressLine1;
			//	model.AddressLine2 = _userinfo.AddressLine2;
			//	model.BriefIntroduction = _userinfo.BriefIntroduction;
			//	model.CityId = _userinfo.CityId;
			//	model.CoverPictureId = _userinfo.CoverPictureId;
			//	model.Email = _userinfo.Email;
			//	model.FirstName = _userinfo.FirstName;
			//	model.Hobbies = _userinfo.Hobbies;
			//	model.IsEmailVerified = _userinfo.IsEmailVerified;
			//	model.IsPhoneVerified = _userinfo.IsPhoneVerified;
			//	model.LastName = _userinfo.LastName;
			//	model.Phone = _userinfo.Phone;
			//	model.ProfilePictureId = _userinfo.ProfilePictureId;
			//	model.Street = _userinfo.Street;
			//	model.Id = _userinfo.Id;
			//	model.user = _userContext.CurrentUser;
			//}

			return View(model);
		}

		public string GetEntityName(TrackerEnabledDbContext.Common.Models.AuditLog log)
		{
			if (log.TypeFullName.Substring(log.TypeFullName.LastIndexOf('.') + 1) == "Product")
			{
				return _productService.GetProductById(Convert.ToInt32(log.RecordId)).Name;
			}

			if (log.TypeFullName.Substring(log.TypeFullName.LastIndexOf('.') + 1) == "Events")
			{
				return _eventService.GetEventById(Convert.ToInt32(log.RecordId)).Title;
			}

			if (log.TypeFullName.Substring(log.TypeFullName.LastIndexOf('.') + 1) == "Blogs")
			{
				return _blogService.GetBlogById(Convert.ToInt32(log.RecordId)).Name;
			}

			return "";
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditMyAccount(UserInfoModel model)
		{
			if (_userContext.CurrentUser == null)
				return LogOff();

			if (ModelState.IsValid)
			{
				//var _userinfo = _userInfoService.GetUserInformationByUserId(model.Id);

				//if (_userinfo != null)
				//{
				//	_userinfo.AddressLine1 = model.AddressLine1;
				//	_userinfo.AddressLine2 = model.AddressLine2;
				//	_userinfo.BriefIntroduction = model.BriefIntroduction;
				//	_userinfo.CityId = model.CityId;
				//	_userinfo.CoverPictureId = model.CoverPictureId;
				//	_userinfo.Email = model.Email;
				//	_userinfo.FirstName = model.FirstName;
				//	_userinfo.Hobbies = model.Hobbies;
				//	_userinfo.IsEmailVerified = model.IsEmailVerified;
				//	_userinfo.IsPhoneVerified = model.IsPhoneVerified;
				//	_userinfo.LastName = model.LastName;
				//	_userinfo.Phone = model.Phone;
				//	_userinfo.ProfilePictureId = model.ProfilePictureId;
				//	_userinfo.Street = model.Street;
				//	_userinfo.UserId = _userContext.CurrentUser.Id;
				//	_userinfo.user = _userContext.CurrentUser;
				//	_userInfoService.Update(_userinfo);
				//}

				return RedirectToAction("MyAccount");
			}

			return View(model);
		}

		#endregion
	}
}