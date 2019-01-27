using EF.Core;
using System.Collections.Generic;
using System.Web.Routing;

namespace EF.Services.Social
{
    public partial interface ISocialModelFactory
    {
        List<SocialAuthenticationMethodModel> PrepareSocialMethodsModel();
        ISocialAuthenticationMethod GetEntryPoint(string assemblyName, string namespaceName, string typeName, string methodName);
        List<SocialAuthenticationMethodModel> PrepareSocialConfigurationModel();
    }
}
