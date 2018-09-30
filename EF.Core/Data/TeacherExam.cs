using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class TeacherExam : BaseEntity
	{
		public int TeacherId { get; set; }
		public int ExamId { get; set; }
		public int ResultStatusId { get; set; }
		public int GradeSystemId { get; set; }
		public double MarksObtained { get; set; }
		public virtual Exam Exam { get; set; }
		public virtual Teacher Teacher { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool BreakAllowed { get; set; }
		public DateTime BreakTime { get; set; }
		public int ClassRoomId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

		public virtual ClassRoom ClassRoom { get; set; }
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
		[NotMapped]
		public GradeSystem GradeSystem
		{
			get
			{
				return (GradeSystem)this.GradeSystemId;
			}
			set
			{
				this.GradeSystemId = (int)value;
			}
		}

	}
}
