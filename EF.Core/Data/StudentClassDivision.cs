using System;

namespace EF.Core.Data
{
	public partial class StudentClassDivision : BaseEntity
	{
		public int StudentId { get; set; }
		public int ClassId { get; set; }
		public int DivisionId { get; set; }
		public string RollNumber { get; set; }
		//public virtual Student Student { get; set; }
		public virtual Class Class { get; set; }
		public string DivisionName { get; set; }
		public string StudentName { get; set; }
		//public virtual Division Division { get; set; }
	}
}
