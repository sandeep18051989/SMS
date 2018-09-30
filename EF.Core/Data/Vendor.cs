using System;

namespace EF.Core.Data
{
	public partial class Vendor : BaseEntity
	{
		public int RegNumber { get; set; }
		public string VendorName { get; set; }
		public string VendorAddress { get; set; }
		public string OfficeContact { get; set; }
		public string MobileContact { get; set; }
		public DateTime Date { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

	}
}
