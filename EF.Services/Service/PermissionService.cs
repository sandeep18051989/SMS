using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public class PermissionService : IPermissionService
    {

        #region Fields

        public readonly IRepository<UserRole> _roleRepository;
        public readonly IRepository<User> _userRepository;
        public readonly IRepository<PermissionRecord> _permissionRepository;
        private readonly IUserContext _userContext;

        #endregion

        #region Const

        public PermissionService(IRepository<UserRole> roleRepository, IRepository<User> userRepository, IRepository<PermissionRecord> permissionRepository, IUserContext userContext)
        {
            this._roleRepository = roleRepository;
            this._userRepository = userRepository;
            this._permissionRepository = permissionRepository;
            this._userContext = userContext;
        }
        #endregion


        #region Permission Members

        public void Insert(PermissionRecord PermissionRecord)
		{
            _permissionRepository.Insert(PermissionRecord);
		}

		public void Update(PermissionRecord PermissionRecord)
		{
            _permissionRepository.Update(PermissionRecord);
		}

        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("role");

            var _permit = _permissionRepository.Table.Where(x => x.Id == id && x.IsSystemDefined == false).FirstOrDefault();
            if (_permit != null)
            {
                _permit.IsDeleted = true;
                _permissionRepository.Update(_permit);
            }
        }

        #endregion

        #region Main Utilities

        public IList<PermissionRecord> GetPermissionsByUserId(int userId)
		{
			if (userId > 0)
			{
                var _user = _userRepository.Table.Where(u => u.Id == userId).FirstOrDefault();
                if (_user != null)
                {
                    var permissions = (from pr in _permissionRepository.Table
                               from r in _roleRepository.Table
                               from u in _userRepository.Table
                               where u.Id == _user.Id && pr.IsDeleted == false
                               select pr).ToList();


                    return permissions;
                }

                return null;
			}
			else
			{
				return null;
			}
		}

        public IList<PermissionRecord> GetPermissionsByRoleId(int roleId)
        {
            if (roleId == 0)
                throw new Exception("Role id Missing");

                var query = _permissionRepository.Table.Where(per=>per.IsDeleted == false).ToList();
                return query.Where(x=>x.PermissionRoles.Any(r=>r.Id == roleId)).ToList();
        }

        public PermissionRecord GetPermissionsByName(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                return _permissionRepository.Table.Where(u => u.SystemName == name && u.IsDeleted == false).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public PermissionRecord GetPermissionById(int id)
        {
            if (id > 0)
            {
                var permit = from r in _permissionRepository.Table
                           where r.Id == id && r.IsDeleted == false
                           select r;

                var query = permit.FirstOrDefault();

                return query;
            }
            else
            {
                return null;
            }
        }

        public IList<PermissionRecord> GetAllPermissions(bool? active)
        {
            return _permissionRepository.Table.Where(x=> (!active.HasValue || x.IsActive == active.Value) && x.IsDeleted == false).OrderBy(x=>x.SystemName).ToList();
        }

        public virtual void DeletePermissions(IList<PermissionRecord> permissions)
        {
            if (permissions == null)
                throw new ArgumentNullException("Permissions not defined in parameter");

            foreach (var _permission in permissions)
            {
                if (!_permission.IsSystemDefined)
                        _permission.IsDeleted = true;

                _permissionRepository.Update(_permission);

            }
        }

        public virtual IList<PermissionRecord> GetPermissionsByIds(int[] permissionIds)
        {
            if (permissionIds == null || permissionIds.Length == 0)
                return new List<PermissionRecord>();

            var query = from r in _permissionRepository.Table
                        where permissionIds.Contains(r.Id)
                        select r;

            var permissions = query.ToList();

            var sortedPermissions = new List<PermissionRecord>();
            foreach (int id in permissionIds)
            {
                var permission = permissions.Find(x => x.Id == id);
                if (permission != null)
                    sortedPermissions.Add(permission);
            }
            return sortedPermissions;
        }

        #endregion

        #region Authorizations

        public virtual bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _userContext.CurrentUser);
        }

        public virtual bool Authorize(PermissionRecord permission, User user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            return Authorize(permission.SystemName, user);
        }

        public virtual bool Authorize(string permissionRecordSystemName)
        {
            return Authorize(permissionRecordSystemName, _userContext.CurrentUser);
        }

        public virtual bool Authorize(string permissionRecordSystemName, User user)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var _roles = user.Roles.Where(cr => cr.IsActive);
            foreach (var role in _roles)
                if (Authorize(permissionRecordSystemName, role))
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        protected virtual bool Authorize(string permissionRecordSystemName, UserRole userRole)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

                foreach (var permission1 in userRole.PermissionRecords)
                    if (permission1.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
        }
        #endregion
    }
}
