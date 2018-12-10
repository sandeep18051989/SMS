using System;

namespace EF.Core.Data
{
	public partial class Vendor : BaseEntity
	{
		public string RegNumber { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string OfficeContact { get; set; }
		public string MobileContact { get; set; }
		public DateTime? Date { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

	}
}
