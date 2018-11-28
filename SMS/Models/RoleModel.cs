using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
    [Validator(typeof(RoleValidation))]
    public partial class RoleModel : BaseEntityModel
	{
		public string RoleName { get; set; }
		public bool IsSystemDefined { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}
}