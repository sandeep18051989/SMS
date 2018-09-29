using SMS.Areas.Admin.Validations;
using SMS.Validations;
using EF.Services;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Areas.Admin.Models
{
    [Validator(typeof(PermissionModelValidator))]
    public class PermissionModel : BaseEntityModel 
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Category { get; set; }
        public bool IsSystemDefined { get; set; }
        public bool Checked { get; set; }
    }
}