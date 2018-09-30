using EF.Core.Data;
using System;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface IRoleService
    {
        void Insert(UserRole user);
        void Update(UserRole user);

        void Delete(int id);

        IList<UserRole> GetAllRoles(bool active=false);
        UserRole GetRoleById(int roleId);
        IList<UserRole> GetRolesByUserId(int userId);

        UserRole GetRoleByName(string roleName);

        void DeleteRoles(IList<UserRole> roles);

        IList<UserRole> GetRolesByIds(int[] roleIds);

        void ToggleRole(int id);

    }
}
