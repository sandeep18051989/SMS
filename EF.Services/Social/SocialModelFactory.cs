using EF.Core;
using EF.Core.Social;
using EF.Services.Social;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Routing;

namespace EF.Services.Social
{
    public partial class SocialModelFactory : ISocialModelFactory
    {
        #region Fields

        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IUserContext _userContext;
        private readonly ISocialPluginService _socialPluginService;

        #endregion

        #region Ctor

        public SocialModelFactory(IOpenAuthenticationService openAuthenticationService,IUserContext userContext, ISocialPluginService socialPluginService)
        {
            this._openAuthenticationService = openAuthenticationService;
            this._userContext = userContext;
            this._socialPluginService = socialPluginService;
        }

        #endregion

        #region Methods

        public virtual List<SocialAuthenticationMethodModel> PrepareSocialMethodsModel()
        {
            var model = new List<SocialAuthenticationMethodModel>();
            var activeSocialPlugins = _socialPluginService.GetSocialPlugins(true);
            foreach (var eam in activeSocialPlugins)
            {
                var eamModel = new SocialAuthenticationMethodModel();
                string actionName = "";
                string controllerName = "";
                RouteValueDictionary routeValues = null;
                var entryClass = GetEntryPoint(eam.AssemblyName, eam.AuthenticationMethodServiceNamespace, eam.AuthenticationMethodService, "GetPublicInfoRoute");
                if (entryClass != null)
                {
                    entryClass.GetPublicInfoRoute(out actionName, out controllerName, out routeValues);
                    eamModel.ActionName = actionName;
                    eamModel.ControllerName = controllerName;
                    eamModel.RouteValues = routeValues;
                    eamModel.DisplayName = eam.DisplayIdentifier;
                    model.Add(eamModel);
                }
            }

            return model;
        }

        public virtual List<SocialAuthenticationMethodModel> PrepareSocialConfigurationModel()
        {
            var model = new List<SocialAuthenticationMethodModel>();
            var activeSocialPlugins = _socialPluginService.GetSocialPlugins(true);
            foreach (var eam in activeSocialPlugins)
            {
                var eamModel = new SocialAuthenticationMethodModel();
                string actionName = "";
                string controllerName = "";
                RouteValueDictionary routeValues = null;
                var entryClass = GetEntryPoint(eam.AssemblyName, eam.AuthenticationMethodServiceNamespace, eam.AuthenticationMethodService, "GetConfigurationRoute");
                if (entryClass != null)
                {
                    entryClass.GetConfigurationRoute(out actionName, out controllerName, out routeValues);
                    eamModel.ActionName = actionName;
                    eamModel.ControllerName = controllerName;
                    eamModel.RouteValues = routeValues;
                    eamModel.DisplayName = eam.DisplayIdentifier;
                    model.Add(eamModel);
                }
            }
            return model;
        }

        public ISocialAuthenticationMethod GetEntryPoint(string assemblyName, string namespaceName, string typeName, string methodName)
        {
            ISocialAuthenticationMethod task = null;
            Type calledType = Type.GetType(namespaceName + "." + typeName + "," + assemblyName);
            var scope = ContextHelper.Current.ContainerManager.Scope();
            if (calledType != null)
            {
                object instance;
                if (!ContextHelper.Current.ContainerManager.TryResolve(calledType, scope, out instance))
                {
                    instance = ContextHelper.Current.ContainerManager.ResolveUnregistered(calledType, scope);
                }
                task = instance as ISocialAuthenticationMethod;
            }
            return task;
            //calledType.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new Object[] { actionname, controllerName, routeValues });
        }

        #endregion
    }
}
