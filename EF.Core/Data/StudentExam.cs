using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;
using System.Collections.Generic;

namespace EF.Core.Data
{
	public partial class StudentExam : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<Comment> _Comments { get; set; }

        public int StudentId { get; set; }
		public int ExamId { get; set; }
		public virtual Exam Exam { get; set; }
		public int ResultStatusId { get; set; }
		public int GradeSystemId { get; set; }
		public double MarksObtained { get; set; }
		public virtual Student Student { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public bool BreakAllowed { get; set; }
		public string BreakTime { get; set; }
		public int ClassRoomId { get; set; }
        public double PassingMarks { get; set; }
        public double MaxMarks { get; set; }


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

        public virtual ICollection<Comment> Comments
        {
            get { return _Comments ?? (_Comments = new List<Comment>()); }
            protected set { _Comments = value; }
        }

    }
}
