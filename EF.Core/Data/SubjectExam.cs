using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class SubjectExam : BaseEntity
	{
		public int SubjectId { get; set; }
		public int ExamId { get; set; }
		private int ResultStatusId { get; set; }
		public double MarksObtained { get; set; }
		public virtual Exam Exam { get; set; }
		public virtual Subject Subject { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

		[NotMapped]
		public ResultStatus ResultStatus
		{
			get
			{
				return (ResultStatus)this.ResultStatusId;
			}
			set
			{
				this.ResultStatusId = (int)value;
			}
		}
	}
}
