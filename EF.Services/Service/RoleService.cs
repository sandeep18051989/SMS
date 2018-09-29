using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public class RoleService : IRoleService
    {
		#region Fields

		public readonly IRepository<UserRole> _roleRepository;
        public readonly IRepository<User> _userRepository;

        #endregion

        #region Const

        public RoleService(IRepository<UserRole> roleRepository, IRepository<User> userRepository)
        {
            this._roleRepository = roleRepository;
            this._userRepository = userRepository;
        }
        #endregion


        #region IRole Members

        public void Insert(UserRole role)
		{
            _roleRepository.Insert(role);
		}

		public void Update(UserRole role)
		{
            _roleRepository.Update(role);
		}

        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("role");

            var _role = _roleRepository.Table.Where(x => x.Id == id && x.IsSystemDefined != true).FirstOrDefault();
            if (_role != null)
            {
                _role.IsDeleted = true;
                _roleRepository.Update(_role);
            }

        }

        #endregion

        #region Main Utilities

        public IList<UserRole> GetRolesByUserId(int userId)
		{
			if (userId > 0)
			{
				return _userRepository.Table.Where(x=>x.Id == userId).FirstOrDefault().Roles.ToList();
			}
			else
			{
				return new List<UserRole>();
			}
		}

        public UserRole GetRoleById(int id)
        {
            if (id > 0)
            {
                var role = from r in _roleRepository.Table
                            where r.Id == id
                           select r;

                var query = role.FirstOrDefault();

                return query;
            }
            else
            {
                return null;
            }
        }

        public IList<UserRole> GetAllRoles(bool active = false)
        {
            var query = _roleRepository.Table.ToList().Where(x=>x.IsDeleted == false).ToList();

            if (active)
                query = query.Where(x => x.IsActive == true).ToList();

            return query;
        }

        public UserRole GetRoleByName(string roleName)
        {
            if (!String.IsNullOrEmpty(roleName))
            {
                var role = from r in _roleRepository.Table
                           where r.RoleName == roleName
                           select r;

                var query = role.FirstOrDefault();

                return query;
            }
            else
            {
                return null;
            }
        }

        public virtual void DeleteRoles(IList<UserRole> roles)
        {
            if (roles == null)
                throw new ArgumentNullException("roles");

            foreach (var _role in roles)
            {
                if(!_role.IsSystemDefined)
                    _role.IsDeleted = true;

                _roleRepository.Update(_role);

            }
        }

        public virtual IList<UserRole> GetRolesByIds(int[] roleIds)
        {
            if (roleIds == null || roleIds.Length == 0)
                return new List<UserRole>();

            var query = from r in _roleRepository.Table
                        where roleIds.Contains(r.Id) && r.IsSystemDefined == false
                        select r;

            var roles = query.ToList();

            var sortedRoles = new List<UserRole>();
            foreach (int id in roleIds)
            {
                var role = roles.Find(x => x.Id == id);
                if (role != null)
                    sortedRoles.Add(role);
            }
            return sortedRoles;
        }

        public void ToggleRole(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("role");

            var _role = _roleRepository.Table.Where(x => x.Id == id && x.IsSystemDefined != true).FirstOrDefault();
            if (_role != null)
            {
                _role.IsActive = !_role.IsActive;
                _roleRepository.Update(_role);
            }

        }

        #endregion
    }
}
