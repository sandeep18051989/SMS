using System;

namespace EF.Core.Data
{
	public partial class Holiday : BaseEntity
	{
		public DateTime Date { get; set; }
		public int DD { get; set; }
		public int MM { get; set; }
		public int YYYY { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public int AcadmicYearId { get; set; }

	}
}
