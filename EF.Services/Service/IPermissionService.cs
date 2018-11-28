using EF.Core.Data;
using System;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface IPermissionService
    {
        void Insert(PermissionRecord permission);
        void Update(PermissionRecord permission);

        void Delete(int id);

        IList<PermissionRecord> GetAllPermissions(bool? onlyActive = null, bool? showSystemDefined = null);

        IList<PermissionRecord> GetPermissionsByUserId(int userId);

        PermissionRecord GetPermissionById(int id);

        PermissionRecord GetPermissionsByName(string name);

        IList<PermissionRecord> GetPermissionsByRoleId(int roleId);

        void DeletePermissions(IList<PermissionRecord> permissions);

        IList<PermissionRecord> GetPermissionsByIds(int[] permissionIds);

        bool Authorize(PermissionRecord permission);

        bool Authorize(PermissionRecord permission, User user);

        bool Authorize(string permissionRecordSystemName);

        bool Authorize(string permissionRecordSystemName, User user);

    }
}
