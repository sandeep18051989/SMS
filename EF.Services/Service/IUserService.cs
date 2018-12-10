using EF.Core.Data;
using System;
using System.Collections.Generic;
using Wibci.CountryReverseGeocode.Models;

namespace EF.Services.Service
{
	public interface IUserService
	{
		void Insert(User user);
		void Update(User user);
		void Delete(int id);
		void InsertLocation(Location location);

		void DeleteUsers(IList<User> users);

		User GetUserByUsername(string username);
		User GetUserByEmail(string email);
		User GetUserById(int userid);
		User GetUserByGuid(Guid userGuid);
		IList<User> GetAllUsers(bool? active = null, bool? approved = null);
        bool CheckUsernameExists(string username, int? id = null);
        string GetUsernameByUser(int id);

        void ApproveUsers(IList<User> users);

		void RejectUsers(IList<User> users);

		IList<User> GetUsersByIds(int[] userIds);

		void ToggleUser(int id);

		IList<User> GetUnApprovedUsers();

		int GetUserCountByLoginDate(DateTime logindate);
	    int GetPendingUserCount();

        LocationInfo GetCountryByLocation(double latitude, double longitude);
		IList<Location> GetUserLocationsByCountry(string country);
		IList<Location> GetAllUserLocations();
	}
}
