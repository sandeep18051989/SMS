using EF.Core.Data;
using System;
using System.Linq;

namespace EF.Services
{
    public static class UserExtensions
    {
        #region User role

        /// <summary>
        /// Gets a value indicating whether user is in a certain user role
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userRoleName">User role system name</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsInUserRole(this User user,
            string userRoleName, bool onlyActiveUserRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(userRoleName))
                throw new ArgumentNullException("userRoleName");

            var result = user.Roles.FirstOrDefault(cr => (!onlyActiveUserRoles || cr.IsActive) && (cr.RoleName == userRoleName)) != null;
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether user is administrator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public static bool IsAdmin(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, "Administrators" , onlyActiveUserRoles);
        }

        #endregion

    }
}
