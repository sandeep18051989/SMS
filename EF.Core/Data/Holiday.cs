using System;

namespace EF.Core.Data
{
	public partial class Holiday : BaseEntity
	{
		public DateTime? Date { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int AcadmicYearId { get; set; }

	}
}
