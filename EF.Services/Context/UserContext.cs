using System;
using System.Web;
using System.Web.Security;
using EF.Core;
using EF.Core.Data;
using EF.Services.Service;

namespace EF.Services.Context
{
	/// <summary>
	/// Work context for web application
	/// </summary>
	public partial class UserContext : IUserContext
	{

		#region Fields
		private const string UserCookieName = "smsuser";
		private const string AcadmicYearCookieName = "acadmicyear";
		private const string SchoolCookieName = "school";

		private readonly IAuthenticationService _authenticationService;
		private readonly IUserService _userService;
		private readonly ISMSService _smsService;
		private readonly HttpContextBase _httpContext;
		private readonly TimeSpan _expiryTimeSpan;

		private User _cachedUser;
		private AcadmicYear _cachedAcadmicYear;
		private School _cachedSchool;
		private bool _cachedIsAdmin;
		#endregion
		#region Constructor
		public UserContext(HttpContextBase httpcontext, IUserService userService, IAuthenticationService authenticationService, ISMSService smsService)
		{
			this._httpContext = httpcontext;
			this._userService = userService;
			this._authenticationService = authenticationService;
			this._expiryTimeSpan = FormsAuthentication.Timeout;
			this._smsService = smsService;
		}

		#endregion

		#region Properties

		public virtual User CurrentUser
		{
			get
			{
				if (_cachedUser != null)
					return _cachedUser;

				User user = null;
				if (_httpContext != null)
				{

					//registered user
					if (user == null)
					{
						user = _authenticationService.GetAuthenticatedUser();
					}

					//validation
					if (user != null)
					{
						if (!user.IsDeleted && user.IsActive)
						{
							SetUserCookie(user.UserGuid);
							_cachedUser = user;
						}
					}
				}

				return _cachedUser;
			}
			set
			{
				SetUserCookie(value.UserGuid);
				_cachedUser = value;
			}
		}

		public virtual School CurrentSchool
		{
			get
			{
				if (_cachedSchool != null)
					return _cachedSchool;

				School school = null;
				if (_httpContext != null)
				{
					//validation
					if (school != null)
					{
						if (!school.IsDeleted && school.IsActive)
						{
							SetUserCookie(school.SchoolGuid);
							_cachedSchool = school;
						}
					}
				}

				return _cachedSchool;
			}
			set
			{
				SetSchoolCookie(value.SchoolGuid);
				_cachedSchool = value;
			}
		}

		public virtual AcadmicYear CurrentAcadmicYear
		{
			get
			{
				if (_cachedAcadmicYear != null)
					return _cachedAcadmicYear;

				AcadmicYear acadmicyear = null;
				if (_httpContext != null)
				{

					if (acadmicyear == null)
					{
						var activeacadmicyear = _smsService.GetActiveAcadmicYear();
						if (activeacadmicyear != null)
						{
							_cachedAcadmicYear = activeacadmicyear;
						}
					}

					//validation
					if (acadmicyear != null)
					{
						if (!acadmicyear.IsDeleted && acadmicyear.IsActive)
						{
							SetAcadmicYearCookie(acadmicyear.Name.Trim());
							_cachedAcadmicYear = acadmicyear;
						}
					}
				}

				return _cachedAcadmicYear;
			}
			set
			{
				SetAcadmicYearCookie(value.Name.Trim());
				_cachedAcadmicYear = value;
			}
		}

		protected virtual HttpCookie GetUserCookie()
		{
			if (_httpContext == null || _httpContext.Request == null)
				return null;

			return _httpContext.Request.Cookies[UserCookieName];
		}

		protected virtual void SetUserCookie(Guid userGuid)
		{
			if (_httpContext != null && _httpContext.Response != null)
			{
				var cookie = new HttpCookie(UserCookieName);
				//cookie.HttpOnly = true;
				cookie.Value = userGuid.ToString();
				if (userGuid == Guid.Empty)
				{
					cookie.Expires = DateTime.Now.AddMonths(-1);
				}
				else
				{
					int cookieExpires = 24 * 365; //TODO make configurable
					cookie.Expires = DateTime.Now.AddHours(cookieExpires);
				}

				_httpContext.Response.Cookies.Remove(UserCookieName);
				_httpContext.Response.Cookies.Add(cookie);
			}
		}

		protected virtual HttpCookie GetAcadmicYearCookie()
		{
			if (_httpContext == null || _httpContext.Request == null)
				return null;

			return _httpContext.Request.Cookies[AcadmicYearCookieName];
		}

		protected virtual void SetAcadmicYearCookie(string acadmicyear)
		{
			if (_httpContext != null && _httpContext.Response != null)
			{
				var cookie = new HttpCookie(AcadmicYearCookieName);

				cookie.Value = acadmicyear.Trim();
				if (String.IsNullOrEmpty(acadmicyear))
				{
					cookie.Expires = DateTime.Now.AddMonths(-1);
				}
				else
				{
					int cookieExpires = 24 * 365;
					cookie.Expires = DateTime.Now.AddHours(cookieExpires);
				}

				_httpContext.Response.Cookies.Remove(AcadmicYearCookieName);
				_httpContext.Response.Cookies.Add(cookie);
			}
			else
			{
				var activeacadmicyear = _smsService.GetActiveAcadmicYear();

				if (activeacadmicyear != null)
				{
					var cookie = new HttpCookie(AcadmicYearCookieName);

					cookie.Value = activeacadmicyear.Name.Trim();
					if (String.IsNullOrEmpty(activeacadmicyear.Name))
					{
						cookie.Expires = DateTime.Now.AddMonths(-1);
					}
					else
					{
						int cookieExpires = 24 * 365;
						cookie.Expires = DateTime.Now.AddHours(cookieExpires);
					}

					_httpContext.Response.Cookies.Remove(AcadmicYearCookieName);
					_httpContext.Response.Cookies.Add(cookie);
				}
			}
		}

		protected virtual HttpCookie GetSchoolCookie()
		{
			if (_httpContext == null || _httpContext.Request == null)
				return null;

			return _httpContext.Request.Cookies[SchoolCookieName];
		}

		protected virtual void SetSchoolCookie(Guid schoolGuid)
		{
			if (_httpContext != null && _httpContext.Response != null)
			{
				var cookie = new HttpCookie(SchoolCookieName);

				cookie.Value = schoolGuid.ToString();
				if (schoolGuid == Guid.Empty)
				{
					cookie.Expires = DateTime.Now.AddMonths(-1);
				}
				else
				{
					int cookieExpires = 24 * 365;
					cookie.Expires = DateTime.Now.AddHours(cookieExpires);
				}

				_httpContext.Response.Cookies.Remove(SchoolCookieName);
				_httpContext.Response.Cookies.Add(cookie);
			}
		}

		public bool IsAdmin
		{
			get
			{
				return _cachedIsAdmin;
			}
			set
			{
				_cachedIsAdmin = value;
			}
		}

		#endregion
	}
}
