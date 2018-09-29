using System;

namespace EF.Core.Data
{
	public partial class ClassHomework : BaseEntity
	{
		public int ClassId { get; set; }
		public int HomeworkId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public virtual Homework Homework { get; set; }
		public virtual Class Class { get; set; }
	}
}
