using System.Web.Mvc;
using EF.Facebook.Core;
using EF.Facebook.Models;
using EF.Services;
using EF.Services.Service;
using EF.Services.Social;
using EF.Core.Data;
using EF.Core;
using EF.Core.Enums;
using EF.Services.Http;

namespace EF.Facebook.Controllers
{
    public class SocialAuthFacebookController : BaseSocialController
    {
        private readonly ISocialSettingService _settingService;
        private readonly IOAuthProviderFacebookAuthorizer _oAuthProviderFacebookAuthorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly SocialSettings _externalAuthenticationSettings;
        private readonly IPermissionService _permissionService;
        private readonly IUserContext _userContext;
        private readonly ISocialPluginService _socialPluginService;

        public SocialAuthFacebookController(ISocialSettingService settingService,
            IOAuthProviderFacebookAuthorizer oAuthProviderFacebookAuthorizer,
            IOpenAuthenticationService openAuthenticationService,
            SocialSettings externalAuthenticationSettings,
            IPermissionService permissionService,
            IUserContext userContext,
            ISocialPluginService socialPluginService)
        {
            this._settingService = settingService;
            this._oAuthProviderFacebookAuthorizer = oAuthProviderFacebookAuthorizer;
            this._openAuthenticationService = openAuthenticationService;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._permissionService = permissionService;
            this._userContext = userContext;
            this._socialPluginService = socialPluginService;
        }
        
        [ChildActionOnly]
        public ActionResult Configure()
        {
            if (!_permissionService.Authorize("ManageSocialSettings"))
                return Content("Access denied");

            var facebookSocialAuthSettings = _settingService.LoadSetting<FacebookSocialAuthSettings>();

            var model = new ConfigurationModel();
            model.ClientKeyIdentifier = facebookSocialAuthSettings.ClientKeyIdentifier;
            model.ClientSecret = facebookSocialAuthSettings.ClientSecret;
            return View("~/EF.Facebook/Views/Configure.cshtml", model);
        }

        [HttpPost]
        //[AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize("ManageSocialSettings"))
                return Content("Access denied");

            if (!ModelState.IsValid)
                return Configure();

            var facebookSocialAuthSettings = _settingService.LoadSetting<FacebookSocialAuthSettings>();

            //save settings
            facebookSocialAuthSettings.ClientKeyIdentifier = model.ClientKeyIdentifier;
            facebookSocialAuthSettings.ClientSecret = model.ClientSecret;

            _settingService.SaveSetting(facebookSocialAuthSettings);

            SuccessNotification("Facebook Settings Saved Successfully!");

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return View("~/Plugins/SocialAuth.Facebook/Views/PublicInfo.cshtml");
        }

        [NonAction]
        private ActionResult LoginInternal(string returnUrl, bool verifyResponse)
        {
            var viewModel = new LoginModel();
            TryUpdateModel(viewModel);

            var result = _oAuthProviderFacebookAuthorizer.Authorize(returnUrl, verifyResponse);
            switch (result.AuthenticationStatus)
            {
                case OpenAuthenticationStatus.Error:
                    {
                        if (!result.Success)
                            foreach (var error in result.Errors)
                                SocialAuthorizerHelper.AddErrorsToDisplay(error);

                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AssociateOnLogon:
                    {
                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                default:
                    break;
            }

            if (result.Result != null) return result.Result;
            return HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }
        
        public ActionResult Login(string returnUrl)
        {
            return LoginInternal(returnUrl, false);
        }

        public ActionResult LoginCallback(string returnUrl)
        {
            return LoginInternal(returnUrl, true);
        }
    }
}