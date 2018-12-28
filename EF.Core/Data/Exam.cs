using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Exam : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<DivisionExam> _DivisionExams { get; set; }
        [NotMapped]
        public virtual ICollection<StudentExam> _StudentExams { get; set; }
        [NotMapped]
        public virtual ICollection<TeacherExam> _TeacherExams { get; set; }

        public string ExamName { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public double PassingMarks { get; set; }
		public double MaxMarks { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

        #region Navigation Properties

        public virtual ICollection<DivisionExam> DivisionExams
        {
            get { return _DivisionExams ?? (_DivisionExams = new List<DivisionExam>()); }
            protected set { _DivisionExams = value; }
        }

        public virtual ICollection<StudentExam> StudentExams
        {
            get { return _StudentExams ?? (_StudentExams = new List<StudentExam>()); }
            protected set { _StudentExams = value; }
        }
        public virtual ICollection<TeacherExam> TeacherExams
        {
            get { return _TeacherExams ?? (_TeacherExams = new List<TeacherExam>()); }
            protected set { _TeacherExams = value; }
        }

        #endregion
    }
}