using EF.Services.Service;
using EF.Services.Social;
using System.Web.Routing;
namespace EF.Facebook
{
    /// <summary>
    /// Facebook externalAuth processor
    /// </summary>
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
            //settings
            var settings = new FacebookSocialAuthSettings
            {
                ClientKeyIdentifier = "",
                ClientSecret = "",
            };
            _settingService.SaveSetting(settings);
        }

        public void Uninstall()
        {
            //settings
            _settingService.DeleteSocialSetting<FacebookSocialAuthSettings>();
        }

        #endregion
    }
}
