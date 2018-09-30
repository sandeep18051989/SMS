using System;

namespace EF.Core.Data
{
	public partial class StudentHomework : BaseEntity
	{
		public int StudentId { get; set; }
		public int HomeworkId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public virtual Homework Homework { get; set; }
		public virtual Student Student { get; set; }
	}
}
