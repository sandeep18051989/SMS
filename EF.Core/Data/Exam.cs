using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Exam : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Comment> _Comments { get; set; }

		public string ExamName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public double PassingMarks { get; set; }
		public double MaxMarks { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

		public virtual ICollection<Comment> Comments
		{
			get { return _Comments ?? (_Comments = new List<Comment>()); }
			protected set { _Comments = value; }
		}

	}
}