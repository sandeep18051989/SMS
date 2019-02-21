using System;
using System.Linq;
using System.Web.Mvc;
using MVCEncrypt;
using EF.Core;
using EF.Core.Data;
using EF.Services;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;
using EF.Core.Enums;
using System.Collections.Generic;

namespace SMS.Controllers
{
    public class AccountController : PublicHttpController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
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
        private readonly ISMSService _smsService;

        public AccountController(IAuthenticationService authenticationService, IUserService userService, IUserContext userContext, ISettingService settingService, ITemplateService templateService, IEmailService emailService, IAuditService auditService, IEventService eventService, ICommentService commentService, IProductService productService, IBlogService blogService, IUrlHelper urlHelper, ISMSService smsService, IRoleService roleService)
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
            this._smsService = smsService;
            this._roleService = roleService;
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
            if (ModelState.IsValid)
            {
                var newUser = new User()
                {
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    IsActive = true,
                    IsApproved = false,
                    IsDeleted = false,
                    Password = model.Password,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    AcceptTermAndConditions = model.AcceptTermAndConditions,
                    AddressLine1 = "",
                    AddressLine2 = "",
                    CityId = 0,
                    CoverPictureId = model.CoverPictureId,
                    Email = model.Email,
                    MiddleName = "",
                    ProfilePictureId = model.ProfilePictureId,
                    UserGuid = Guid.NewGuid(),
                    UserId = 1
                };

                var defaultRole = _roleService.GetRoleByName("General");
                if (defaultRole != null)
                    newUser.Roles.Add(defaultRole);

                _userService.Insert(newUser);

                var verificationLink = Url.ActionEnc("VerificationUrl", "EmailVerification", new { email = newUser.Email, code = newUser.UserGuid });
                // Send Notification To The Admin
                var setting = _settingService.GetSettingByKey("FromEmail");
                if (!String.IsNullOrEmpty(setting.Value))
                    _emailService.SendUserEmailVerificationMessage(newUser, verificationLink);

                SuccessNotification("You Are Registered Successfully. Please Check Your Inbox For Verification.");
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
        public ActionResult Login(LoginModel model)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUsername(model.Email);

                if (user == null)
                    user = _userService.GetUserByEmail(model.Email);

                if (user != null && user.IsApproved)
                {
                    if (user.Password == model.Password)
                    {
                        // Update Last Login Date
                        user.LastLoginDate = DateTime.Now;
                        _userService.Update(user);

                        // Impersonate Relation
                        var teacher = _smsService.GetTeacherByImpersonateId(user.Id);
                        var student = _smsService.GetStudentByImpersonateId(user.Id);

                        //sign in customer
                        _authenticationService.SignIn(user, model.RememberMe);

                        // Send Notification To The User
                        //var template = _settingService.GetSettingByKey("UserSignInAttempt");
                        //if (template != null)
                        //{
                        //    var Template = _templateService.GetTemplateByName(template.Value);

                        //    var tokens = new List<DataToken>();
                        //    _templateService.AddUserTokens(tokens, user);

                        //    foreach (var dt in tokens)
                        //    {
                        //        Template.BodyHtml = CodeHelper.Replace(Template.BodyHtml.ToString(), $"[{dt.SystemName}]", dt.Value, StringComparison.InvariantCulture);
                        //    }

                        //    _emailService.
                        //}

                        _userContext.CurrentUser = user;
                        _userContext.CurrentSchool = _smsService.GetSchoolById(1);

                        // Teacher
                        if (teacher != null)
                        {
                            if (ViewData["ReturnUrl"] == null)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return Redirect(ViewData["ReturnUrl"].ToString());
                            }
                        }
                        // Student
                        else if (student != null)
                        {
                            if (ViewData["ReturnUrl"] == null)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return Redirect(ViewData["ReturnUrl"].ToString());
                            }
                        }
                        // Other Users
                        else
                        {

                            if (ViewData["ReturnUrl"] == null)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return Redirect(ViewData["ReturnUrl"].ToString());
                            }
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
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Emailaddress))
            {
                model.Success = false;
                model.Message = "Please enter a valid email address.";
                return View(model);
            }

            var user = _userService.GetUserByEmail(model.Emailaddress);
            if (user != null)
            {
                // Get Forgot Password Template
                var configSetting = _settingService.GetSettingByKey("ForgotTemplate");
                if (configSetting != null)
                {
                    var templateName = configSetting.Value;
                    var template = _templateService.GetTemplateByName(templateName);
                    if (template != null)
                    {
                        var _lstTokens = template.Tokens.ToList();
                        _templateService.AddUserTokens(_lstTokens, user);

                        string passwordLink = Url.ActionEnc("ForgotPassword", "ResetPassword", new { @email = user.Email, @code = user.UserGuid.ToString(), @isreset = false });
                        _emailService.SendUserForgotPasswordMessage(user, passwordLink);
                        SuccessNotification("A password change request has been sent on your email address. Please follow instructions written in the mail.");
                    }
                }
                else
                {
                    ErrorNotification("Configuration Template setting not available, contact administrator.");
                }
            }

            return RedirectToAction("Login");
        }

        #region User Information

        public JsonResult CheckEmailExists(string Email)
        {
            return Json(!_userService.CheckEmailExists(Email), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUsernameExists(string Username)
        {
            return Json(!_userService.CheckUsernameExists(Username), JsonRequestBehavior.AllowGet);
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

        public ActionResult EmailVerification(string email, Guid code)
        {
            var model = new VerificationModel();

            if (string.IsNullOrEmpty(email))
            {
                model.Success = false;
                model.Message = "User Email Does Not Exists In Our Records!";
                return View(model);
            }

            if (string.IsNullOrEmpty(code.ToString()))
            {
                model.Success = false;
                model.Message = "User Identification Not Met, Please Contact Support!";
                return View(model);
            }

            var registeredUser = _userService.GetUserByEmail(email);
            if (registeredUser == null)
            {
                model.Success = false;
                model.Message = "User Email Does Not Exists In Our Records!";
                return View(model);
            }
            else
            {
                if (registeredUser.UserGuid.ToString() != code.ToString())
                {
                    model.Success = false;
                    model.Message = "User Identification Not Met, Please Contact Support!";
                    return View(model);
                }
                else if (registeredUser.IsApproved)
                {
                    model.Success = false;
                    model.Message = "Email address already verified! You can directly login&nbsp;<a title='Login' href='/Account/Login'><u>here</u></a>&nbsp;";
                    return View(model);
                }
                else
                {
                    registeredUser.IsApproved = true;
                    _userService.Update(registeredUser);
                    model.Success = true;
                    model.Message = "Email address successfully verified! Click&nbsp;<a title='Login' href='/Account/Login'><u>here</u></a>&nbsp;to login.";
                    return View(model);
                }
            }
        }

        public ActionResult ResetPassword(string email, Guid code, bool isreset)
        {
            var model = new ResetPasswordModel();
            model.IsReset = isreset;
            model.EmailAddress = email;

            if (string.IsNullOrEmpty(email))
            {
                model.Success = false;
                model.Message = "User Email Does Not Exists In Our Records!";
                return View(model);
            }

            if (string.IsNullOrEmpty(code.ToString()))
            {
                model.Success = false;
                model.Message = "User Identification Not Met, Please Contact Support!";
                return View(model);
            }

            var registeredUser = _userService.GetUserByEmail(email);
            if (registeredUser == null)
            {
                model.Success = false;
                model.Message = "User Email Does Not Exists In Our Records!";
                return View(model);
            }
            else
            {
                if (registeredUser.UserGuid.ToString() != code.ToString())
                {
                    model.Success = false;
                    model.Message = "User Identification Not Met, Please Contact Support!";
                    return View(model);
                }
                else
                {
                    model.Success = true;
                    model.Message = "";
                    return View(model);
                }
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var user = _userService.GetUserByEmail(model.EmailAddress);
            if (user != null)
            {
                // Get Forgot Password Template
                var configSetting = _settingService.GetSettingByKey("ResetPassword");
                if (configSetting != null)
                {
                    var templateName = configSetting.Value;
                    var template = _templateService.GetTemplateByName(templateName);
                    if (template != null)
                    {
                        var _lstTokens = template.Tokens.ToList();
                        _templateService.AddUserTokens(_lstTokens, user);

                        _emailService.SendUserPasswordChangedMessage(user);
                        SuccessNotification("Password successfully changed.");
                    }
                }
                else
                {
                    ErrorNotification("Configuration Template setting not available, contact administrator.");
                }
            }

            return RedirectToAction("Login");
        }

        #endregion
    }
}