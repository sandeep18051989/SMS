using EF.Services.Service;
using EF.Services.Social;
using System.Web.Routing;
namespace EF.Facebook
{
    public class FacebookSocialAuthMethod : ISocialAuthenticationMethod
    {
        #region Fields

        private readonly ISocialSettingService _settingService;

        #endregion

        #region Ctor

        public FacebookSocialAuthMethod(ISocialSettingService settingService)
        {
            this._settingService = settingService;
        }

        #endregion

        #region Methods
        
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "SocialAuthFacebook";
            routeValues = new RouteValueDictionary { { "Namespaces", "EF.Facebook.Controllers" }, { "area", "Admin" } };
        }

        public void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Info";
            controllerName = "SocialAuthFacebook";
            routeValues = new RouteValueDictionary { { "Namespaces", "EF.Facebook.Controllers" }, { "area", null } };
        }

        public void Install()
        {
            var keyIdentifier = _settingService.GetSocialSettingByKey<FacebookSocialAuthSettings>("ClientKeyIdentifier");
            var clientSecret = _settingService.GetSocialSettingByKey<FacebookSocialAuthSettings>("ClientSecret");
            if (keyIdentifier == null && clientSecret == null)
            {
                var settings = new FacebookSocialAuthSettings
                {
                    ClientKeyIdentifier = "",
                    ClientSecret = ""
                };
                _settingService.SaveSetting(settings);
            }
        }

        public void Uninstall()
        {
            _settingService.DeleteSocialSetting<FacebookSocialAuthSettings>();
        }

        #endregion
    }
}
