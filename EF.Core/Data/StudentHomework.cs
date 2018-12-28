using EF.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class StudentHomework : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<Comment> _Comments { get; set; }

        public int StudentId { get; set; }
		public int HomeworkId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public int StudentHomeworkStatusId { get; set; }
        public int TeacherApprovalStatusId { get; set; }
        public virtual Homework Homework { get; set; }
		public virtual Student Student { get; set; }

        [NotMapped]
        public StudentHomeWorkStatus StudentHomeWorkStatus
        {
            get
            {
                return (StudentHomeWorkStatus)this.StudentHomeworkStatusId;
            }
            set
            {
                this.StudentHomeworkStatusId = (int)value;
            }
        }
        [NotMapped]
        public TeacherApprovalStatus TeacherApprovalStatus
        {
            get
            {
                return (TeacherApprovalStatus)this.TeacherApprovalStatusId;
            }
            set
            {
                this.TeacherApprovalStatusId = (int)value;
            }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return _Comments ?? (_Comments = new List<Comment>()); }
            protected set { _Comments = value; }
        }

    }
}
