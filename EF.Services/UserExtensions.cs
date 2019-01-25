using EF.Core.Data;
using System;
using System.Linq;

namespace EF.Services
{
    public static class UserExtensions
    {
        #region User role

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

        public static bool IsAdmin(this User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, "Administrators" , onlyActiveUserRoles);
        }

        public static string GetFullName(this User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var firstName = user.FirstName;
            var lastName = user.LastName;

            string fullName = "";
            if (!String.IsNullOrWhiteSpace(firstName) && !String.IsNullOrWhiteSpace(lastName))
                fullName = string.Format("{0} {1}", firstName, lastName);
            else
            {
                if (!String.IsNullOrWhiteSpace(firstName))
                    fullName = firstName;

                if (!String.IsNullOrWhiteSpace(lastName))
                    fullName = lastName;
            }
            return fullName;
        }

        #endregion

    }
}
