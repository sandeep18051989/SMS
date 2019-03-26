using System.Collections.Generic;
using EF.Core.Data;
namespace EF.Services.Service
{
	public interface ILicenseService
    {
		void Insert(License licenses);
		void Update(License licenses);
		void Delete(int id);
		IList<License> GetAllLicenses();
		License GetLicenseById(int id);
		IList<License> GetLicensesByUser(int userid);
		License GetLicenseByName(string name);
        IList<License> GetLatestLicenses(int? exceptlicenseid = null, int userid = 0);
        IList<License> GetOlderLicenses(int? exceptlicenseid = null, int userid = 0);
        bool IsUserLicenseger(int userid);
        int GetLicenseCountByUser(int userid);
        bool IsValidLicense(string licenseKey, string Url);

    }
}
