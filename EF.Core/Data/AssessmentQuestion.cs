using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class AssessmentQuestion : BaseEntity
	{
		public int AssessmentId { get; set; }
		public int QuestionId { get; set; }
		public int DisplayOrder { get; set; }
		public double Marks { get; set; }
		public DateTime? SolveTime { get; set; }
		public bool IsTimeBound { get; set; }
		public double NegativeMarks { get; set; }
		public double RightMarks { get; set; }
		public virtual Assessment Assessment { get; set; }
		public virtual Question Question { get; set; }
	}
}
