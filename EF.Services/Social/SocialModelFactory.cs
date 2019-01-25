using EF.Core;
using EF.Services.Social;
using System.Collections.Generic;
using System.Web.Routing;

namespace EF.Services.Social
{
    public partial class SocialModelFactory : ISocialModelFactory
    {
		#region Fields

        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IUserContext _userContext;

        #endregion

        #region Ctor

        public SocialModelFactory(IOpenAuthenticationService openAuthenticationService,
            IUserContext userContext)
        {
            this._openAuthenticationService = openAuthenticationService;
            this._userContext = userContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the external authentication method model
        /// </summary>
        /// <returns>List of the external authentication method model</returns>
        public virtual List<SocialAuthenticationMethodModel> PrepareSocialMethodsModel()
        {
            var model = new List<SocialAuthenticationMethodModel>();

            foreach (var eam in _openAuthenticationService.LoadActiveSocialAuthenticationMethods(_userContext.CurrentUser))
            {
                var eamModel = new SocialAuthenticationMethodModel();

                string actionName;
                string controllerName;
                RouteValueDictionary routeValues;
                eam.GetPublicInfoRoute(out actionName, out controllerName, out routeValues);
                eamModel.ActionName = actionName;
                eamModel.ControllerName = controllerName;
                eamModel.RouteValues = routeValues;
                model.Add(eamModel);
            }

            return model;
        }

        #endregion
    }
}
