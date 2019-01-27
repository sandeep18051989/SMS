using EF.Core.Data;
using EF.Core.Social;
using System;
using System.Collections.Generic;
namespace EF.Services.Social
{
    public interface ISocialPluginService
    {
        IEnumerable<SocialProvider> GetSocialPlugins(bool? onlyActive = null);
    }
}
