using EF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public partial class PermissionRecordModel  : BaseEntityModel
    {
        public PermissionRecordModel()
        {
            Roles = new List<RoleModel>();
        }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public bool IsSystemDefined { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public IList<RoleModel> Roles { get; set; }
    }
}