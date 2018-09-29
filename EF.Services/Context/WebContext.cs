using EF.Services.Http;
using EF.Services.Service;
using System;
using System.Linq;

namespace EF.Services.Context
{
    public partial class WebContext : IWebContext
    {
        private readonly IUrlHelper _webHelper;
        private readonly ISettingService _settingService;
        private string _cachedStoreUrl;

        public WebContext(IUrlHelper webHelper, ISettingService settingService)
        {
            this._webHelper = webHelper;
            this._settingService = settingService;
        }

        public virtual string CurrentStoreUrl
        {
            get
            {
                if (!String.IsNullOrEmpty(_cachedStoreUrl))
                    return _cachedStoreUrl;

                //ty to determine the current store by HTTP_HOST
                var host = _webHelper.ServerVariables("HTTP_HOST");
                var store = _settingService.GetSettings().FirstOrDefault(x=>x.Name == "WebContextUrl").Value;

                if (String.IsNullOrEmpty(store))
                    throw new Exception("Website Host Url Not Found");

                _cachedStoreUrl = store;
                return _cachedStoreUrl;
            }
        }
    }
}
