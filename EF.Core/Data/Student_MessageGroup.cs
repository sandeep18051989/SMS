using System;

namespace EF.Core.Data
{
	public partial class Student_MessageGroup : BaseEntity
	{
		public int MessageGroupId { get; set; }
		public int StudentId { get; set; }
		public DateTime DDate { get; set; }
		
		public virtual Student Student { get; set; }
		public virtual MessageGroup MessageGroup { get; set; }
	}
}
