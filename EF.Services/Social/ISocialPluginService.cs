using EF.Core.Data;
using EF.Core.Social;
using System;
using System.Collections.Generic;
namespace EF.Services.Social
{
    public interface ISocialPluginService
    {
        IEnumerable<Type> GetSocialPlugins<T>(User user = null, bool? onlyActive=null) where T : class, ISocial;
    }
}
