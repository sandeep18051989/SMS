using EF.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SMS.Controllers
{
    public class LicenseController : ApiController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly ILicenseService _licenseService;

        #endregion Fileds

        #region Constructor

        public LicenseController(IUserService userService, ILicenseService licenseService)
        {
            this._userService = userService;
            this._licenseService = licenseService;
        }

        #endregion

        // GET: api/License
        public bool IsValidLicense(string licenseKey, string Url)
        {
            if (string.IsNullOrEmpty(licenseKey) || string.IsNullOrEmpty(Url))
                return false;

            _licenseService.
        }

        // GET: api/License/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/License
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/License/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/License/5
        public void Delete(int id)
        {
        }
    }
}
