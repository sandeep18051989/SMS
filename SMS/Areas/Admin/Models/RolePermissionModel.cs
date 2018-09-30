using SMS.Areas.Admin.Validations;
using SMS.Validations;
using EF.Services;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Areas.Admin.Models
{
    [Validator(typeof(RolePermissionValidator))]
    public class RolePermissionModel : BaseEntityModel 
    {
        public RolePermissionModel()
        {
            AvailablePermissions = new List<PermissionModel>();
            AvailableRoles = new List<SelectListItem>();
        }
        public string RoleName { get; set; }
        public int RoleId { get; set; }

        public IList<PermissionModel> AvailablePermissions {get; set;}

        public IList<SelectListItem> AvailableRoles { get; set; }

	    public int[] Selectedpermissions { get; set; }
    }
}