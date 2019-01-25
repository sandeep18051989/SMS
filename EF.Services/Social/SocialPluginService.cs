using EF.Core.Data;
using EF.Core.Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EF.Services.Social
{
    public class SocialPluginService : ISocialPluginService
    {      
        #region Methods

        public virtual IEnumerable<Type> GetSocialPlugins<T>(User user = null, bool? onlyActive=null) where T : class, ISocial
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(ISocial))).Select(x => x.);
            return types;
        }

        #endregion
    }
}
