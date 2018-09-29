using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Data
{
    public partial class PermissionRecord : BaseEntity
    {
        [NotMapped]
        public virtual ICollection<UserRole> _PermissionRoles { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public bool IsSystemDefined { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserRole> PermissionRoles
        {
            get { return _PermissionRoles ?? (_PermissionRoles = new List<UserRole>()); }
            protected set { _PermissionRoles = value; }
        }

    }
}
