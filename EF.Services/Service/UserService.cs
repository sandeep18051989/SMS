using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using GoogleMaps.LocationServices;
using Wibci.CountryReverseGeocode;
using Wibci.CountryReverseGeocode.Models;

namespace EF.Services.Service
{
	public class UserService : IUserService
	{
		#region Fields

		public readonly IRepository<User> _userRepository;
		public readonly IRepository<Location> _locationRepository;
		public readonly ICountryReverseGeocodeService _reverseLocationService;

		#endregion

		#region Const

		public UserService(IRepository<User> userRepository, IRepository<Location> locationRepository, ICountryReverseGeocodeService reverseLocationService)
		{
			this._userRepository = userRepository;
			this._locationRepository = locationRepository;
			this._reverseLocationService = reverseLocationService;
		}
		#endregion


		#region IUser Members

		public void Insert(User user)
		{
			_userRepository.Insert(user);
		}

		public void Update(User user)
		{
			_userRepository.Update(user);
		}

		public void Delete(int id)
		{
			_userRepository.Delete(id);
		}

		public void InsertLocation(Location location)
		{
			_locationRepository.Insert(location);
		}

		#endregion

		#region Main Utilities

		public virtual User GetUserByGuid(Guid userGuid)
		{
			if (userGuid == Guid.Empty)
				return null;

			var query = from c in _userRepository.Table
							where c.UserGuid == userGuid
							orderby c.Id
							select c;

			var user = query.FirstOrDefault();
			return user;
		}

		public User GetUserByUsername(string username)
		{
			if (string.IsNullOrEmpty(username))
				throw new Exception("Username is missing");

			var user = from c in _userRepository.Table
						  orderby c.Id
						  where c.UserName.Trim().ToLower() == username.Trim().ToLower()
						  select c;
			var query = user.FirstOrDefault();
			return query;
		}

		public User GetUserByEmail(string email)
		{
			if (string.IsNullOrEmpty(email))
				throw new Exception("Email address is missing");

			var user = from c in _userRepository.Table
						  orderby c.Id
						  where c.Email.Trim().ToLower() == email.Trim().ToLower()
						  select c;
			var query = user.FirstOrDefault();
			return query;
		}

		public User GetUserById(int userid)
		{
			if (userid > 0)
			{
				return _userRepository.Table.FirstOrDefault(u => u.Id == userid);
			}
			else
			{
				return null;
			}
		}

		public IList<User> GetAllUsers(bool? active = null, bool? approved = null)
		{
			var allUsers = _userRepository.Table.Where(x => x.IsDeleted == false).ToList();

			if (active.HasValue)
				allUsers = allUsers.Where(x => x.IsActive == active).ToList();

			if (approved.HasValue)
				allUsers = allUsers.Where(x => x.IsApproved == approved).ToList();

			return allUsers;
		}

		public IList<User> GetUnApprovedUsers()
		{
			return _userRepository.Table.Where(x => x.IsDeleted == false && !x.IsApproved).ToList();
		}

		public virtual void ApproveUsers(IList<User> users)
		{
			if (users == null)
				throw new ArgumentNullException("users");

			foreach (var _us in users)
			{
				_us.IsApproved = true;
				_userRepository.Update(_us);

			}
		}

		public virtual void RejectUsers(IList<User> users)
		{
			if (users == null)
				throw new ArgumentNullException("users");

			foreach (var _us in users)
			{
				_us.IsApproved = false;
				_us.IsDeleted = true;
				_userRepository.Update(_us);

			}
		}

		public virtual IList<User> GetUsersByIds(int[] userIds)
		{
			if (userIds == null || userIds.Length == 0)
				return new List<User>();

			var query = from r in _userRepository.Table
							where userIds.Contains(r.Id)
							select r;

			var users = query.ToList();

			var sortedUsers = new List<User>();
			foreach (int id in userIds)
			{
				var _user = users.Find(x => x.Id == id);
				if (_user != null)
					sortedUsers.Add(_user);
			}
			return sortedUsers;
		}

		public virtual void DeleteUsers(IList<User> users)
		{
			if (users == null)
				throw new ArgumentNullException("users");

			foreach (var _user in users)
			{
				if (_user.Id != 1)
					_user.IsDeleted = true;

				_userRepository.Update(_user);

			}
		}

		public void ToggleUser(int id)
		{
			if (id == 0)
				throw new ArgumentNullException("user");

			var user = _userRepository.Table.FirstOrDefault(x => x.Id == id && x.Id != 1);
			if (user != null)
			{
				user.IsActive = !user.IsActive;
				_userRepository.Update(user);
			}

		}

		public int GetUserCountByLoginDate(DateTime logindate)
		{
			if (logindate == null)
				throw new ArgumentNullException("user");

			var query = _userRepository.Table.ToList();
			var lstUsers = new List<User>();
			foreach (var q in query)
			{
				if (q.LastLoginDate.HasValue)
				{
					if (q.LastLoginDate.Value.Date == logindate.Date)
						lstUsers.Add(q);
				}
			}
			return lstUsers.ToList().Count;
		}

		public LocationInfo GetCountryByLocation(double latitude, double longitude)
		{
			return _reverseLocationService.FindCountry(new GeoLocation() { Latitude = latitude, Longitude = longitude, Description = "" });
		}

		public IList<Location> GetUserLocationsByCountry(string country)
		{
			if (String.IsNullOrEmpty(country))
				throw new Exception("Country is missing.");

			var _locations = _locationRepository.Table.ToList().Where(l => l.UserId != 0 && l.Area.Contains(country)).GroupBy(l => l.UserId).Select(l => new Location()
			{
				Address = l.FirstOrDefault().Address,
				CreatedOn = l.FirstOrDefault().CreatedOn,
				Host = l.FirstOrDefault().Host,
				Id = l.FirstOrDefault().Id,
				Latitude = l.FirstOrDefault().Latitude,
				Area = l.FirstOrDefault().Area,
				Longitude = l.FirstOrDefault().Longitude,
				UserId = l.Key
			}).ToList();

			return _locations;
		}

		public IList<Location> GetAllUserLocations()
		{
			return _locationRepository.Table.ToList();
		}

		#endregion

		#region Analytics


		#endregion
	}
}
