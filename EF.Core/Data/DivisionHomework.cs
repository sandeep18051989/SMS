using System;

namespace EF.Core.Data
{
	public partial class DivisionHomework : BaseEntity
	{
		public int DivisionId { get; set; }
		public int HomeworkId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public virtual Homework Homework { get; set; }
		public virtual Division Division { get; set; }
	}
}
