using System;

namespace EF.Core.Data
{
	public partial class Subject : BaseEntity
	{
		public string Name { get; set; }
		public Guid SubjectUniqueId { get; set; }
		public string Code { get; set; }
		public string Remarks { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
	}
}
