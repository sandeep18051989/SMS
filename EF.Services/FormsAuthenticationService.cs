using System;
using System.Web;
using System.Web.Security;
using EF.Core.Data;
using EF.Services.Service;
using System.Collections.Generic;
using System.Collections;
using System.Web.Caching;

namespace EF.Services
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly IUserService _userService;
        private readonly TimeSpan _expirationTimeSpan;

        private User _cachedUser;

        /// <summary>
        /// Cunstructor
        /// </summary>
		  /// <param name="httpContext">HTTP context</param>
		public FormsAuthenticationService(HttpContextBase httpcontext, IUserService userService)
        {
			  this._httpContext = httpcontext;
            this._userService = userService;
           this._expirationTimeSpan = FormsAuthentication.Timeout;
        }

        protected virtual User GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var uId = Convert.ToInt32(ticket.UserData);

            if (uId == 0)
                return null;

            var user = _userService.GetUserById(uId);

            return user;
        }

        public virtual void SignIn(User user, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.UserName,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                user.Id.ToString(),
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _cachedUser = user;
        }

        public virtual void SignOut()
        {
            _cachedUser = null;
            FormsAuthentication.SignOut();
        }

        public virtual User GetAuthenticatedUser()
        {
            if (_cachedUser != null)
                return _cachedUser;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var user = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
            if (user != null && user.IsActive && user.IsApproved)
                _cachedUser = user;
            return _cachedUser;
        }
    }
}