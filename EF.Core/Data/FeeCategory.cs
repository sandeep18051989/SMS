using System;

namespace EF.Core.Data
{
	public partial class FeeCategory : BaseEntity
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public int ClassDivisionId { get; set; }
		public double FeeAmount { get; set; }
		public DateTime? PeriodFrom { get; set; }
		public DateTime? PeriodTo { get; set; }
		public virtual Category Category { get; set; }
		public virtual ClassRoomDivision ClassDivision { get; set; }
        public virtual AcadmicYear AcadmicYear { get; set; }
        public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public int AcadmicYearId { get; set; }
	}
}
