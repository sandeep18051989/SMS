using EF.Core;
using EF.Core.Data;
using EF.Core.Social;
using EF.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EF.Services.Social
{
    public class SocialPluginService : ISocialPluginService
    {
        #region Fields

        private readonly IRepository<SocialProvider> _providerRepository;
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Const

        public SocialPluginService(IRepository<SocialProvider> providerRepository, IUrlHelper urlHelper)
        {
            this._providerRepository = providerRepository;
            this._urlHelper = urlHelper;
        }

        #endregion
        #region Methods

        public IEnumerable<SocialProvider> GetSocialPlugins(bool? onlyActive=null)
        {
            return _providerRepository.Table.Where(x => x.IsActive == true).ToList();
        }

        #endregion
    }
}
