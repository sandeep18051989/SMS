using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMS.Models
{
    [Validator(typeof(RoleValidation))]
    public partial class RoleModel : BaseEntityModel
	{
        public RoleModel()
        {
            AvailableAcadmicYears = new List<SelectListItem>();
        }
        public string RoleName { get; set; }
		public bool IsSystemDefined { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public IList<SelectListItem> AvailableAcadmicYears { get; set; }
	}
}