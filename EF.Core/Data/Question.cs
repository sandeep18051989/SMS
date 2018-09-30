using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class Question : BaseEntity
	{
		public string Name { get; set; }
		public string Explanation { get; set; }
		public int QuestionTypeId { get; set; }
		public int? SubjectId { get; set; }
		public Guid QuestionGuid { get; set; }
		public DateTime? SolveTime { get; set; }
		public int DifficultyLevelId { get; set; }
		public double RightMarks { get; set; }
		public double NegativeMarks { get; set; }
		public bool IsTimeBound { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		[NotMapped]
		public DifficultyLevel DifficultyLevel
		{
			get
			{
				return (DifficultyLevel)this.DifficultyLevelId;
			}
			set
			{
				this.DifficultyLevelId = (int)value;
			}
		}

		public virtual QuestionType QuestionType { get; set; }
		public virtual Subject Subject { get; set; }

	}
}
