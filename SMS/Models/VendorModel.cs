using EF.Core.Data;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
	[Validator(typeof(VendorModelValidator))]
	public partial class VendorModel : BaseEntityModel
	{
		public int RegNumber { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string OfficeContact { get; set; }
		public string MobileContact { get; set; }
		public string VDate { get; set; }
		public int AcadmicYearId { get; set; }
		public int ProductCount { get; set; }
		public bool Selected { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}
}
