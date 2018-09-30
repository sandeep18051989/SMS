using System;

namespace EF.Core.Data
{
	public partial class DivisionSubject : BaseEntity
	{
		public int SubjectId { get; set; }
		public int DivisionId { get; set; }
		public virtual Subject Subject { get; set; }
		public virtual Division Division { get; set; }
	}
}
