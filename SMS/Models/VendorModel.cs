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
		public string VendorName { get; set; }
		public string VendorAddress { get; set; }
		public string OfficeContact { get; set; }
		public string MobileContact { get; set; }
		public string VDate { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}
}
