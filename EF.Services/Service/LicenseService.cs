using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using System.Data.Entity;

namespace EF.Services.Service
{
	public class LicenseService : ILicenseService
    {
		public readonly IRepository<License> _licenseRepository;
		public LicenseService(IRepository<License> licenseRepository)
		{
			_licenseRepository = licenseRepository;
		}
		#region ILicenseService Members

		public void Insert(License licenses)
		{
			_licenseRepository.Insert(licenses);
		}

		public void Update(License licenses)
		{
			_licenseRepository.Update(licenses);
		}

		public void Delete(int id)
		{
			_licenseRepository.Delete(id);
		}

		#endregion

		#region Methods

		public virtual IList<License> GetAllLicenses()
		{
			return _licenseRepository.GetAll().ToList();
		}

		public License GetLicenseById(int id)
		{
			if (id == 0)
				throw new Exception("License Id Not Specified.");

			return _licenseRepository.GetByID(id);
		}

		public virtual IList<License> GetLicensesByUser(int userid)
		{
			if (userid == 0)
				throw new Exception("User Id Not Specified.");

			return _licenseRepository.Table.Where(x => x.UserId == userid).OrderByDescending(x => x.CreatedOn).ToList();
		}

		public License GetLicenseByName(string name)
		{
			if (!string.IsNullOrEmpty(name))
				return _licenseRepository.Table.FirstOrDefault(a => a.Name.Trim().ToLower() == name.ToLower());

			return null;
		}

        public IList<License> GetLatestLicenses(int? exceptlicenseid=null, int userid=0)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, totalDays).Date;
            return _licenseRepository.Table.Where(x => (!exceptlicenseid.HasValue || x.Id != exceptlicenseid.Value) && (userid == 0 || x.UserId == userid) && ((DbFunctions.TruncateTime(x.CreatedOn) >= startDate && DbFunctions.TruncateTime(x.CreatedOn) <= endDate))).ToList();
        }

        public IList<License> GetOlderLicenses(int? exceptlicenseid = null, int userid = 0)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            return _licenseRepository.Table.Where(x => (!exceptlicenseid.HasValue || x.Id != exceptlicenseid.Value) && (userid == 0 || x.UserId == userid) && ((DbFunctions.TruncateTime(x.CreatedOn) < startDate))).ToList();
        }

        public int GetLicenseCountByUser(int userid)
        {
            return _licenseRepository.Table.Count(x => x.UserId == userid);
        }

        public bool IsUserLicenseger(int userid)
        {
            return _licenseRepository.Table.Any(x => x.UserId == userid);
        }

        public bool IsValidLicense(string licenseKey, string Url)
        {
            DateTime actionDate = DateTime.Now.Date;
            return _licenseRepository.Table.Any(x => (x.LicenseKey.Trim().ToLower() == licenseKey.Trim().ToLower() && x.LicenseUrl.Trim().ToLower() == Url.Trim().ToLower()) && (x.LicenseStartDate != null && (DbFunctions.TruncateTime(x.LicenseStartDate) >= actionDate)) && (x.LicenseEndDate != null && (DbFunctions.TruncateTime(x.LicenseEndDate) <= actionDate)));
        }

        #endregion

    }
}
