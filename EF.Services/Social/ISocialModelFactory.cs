using EF.Core;
using System.Collections.Generic;

namespace EF.Services.Social
{
    public partial interface ISocialModelFactory
    {
        List<SocialAuthenticationMethodModel> PrepareSocialMethodsModel();
    }
}
