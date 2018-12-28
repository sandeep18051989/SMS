using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Division : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<DivisionExam> _DivisionExams { get; set; }

        [NotMapped]
        public virtual ICollection<DivisionHomework> _DivisionHomeworks { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
        public int DisplayOrder { get; set; }

        #region Navigation Properties
        public virtual ICollection<DivisionExam> DivisionExams
        {
            get { return _DivisionExams ?? (_DivisionExams = new List<DivisionExam>()); }
            protected set { _DivisionExams = value; }
        }

        public virtual ICollection<DivisionHomework> DivisionHomeworks
        {
            get { return _DivisionHomeworks ?? (_DivisionHomeworks = new List<DivisionHomework>()); }
            protected set { _DivisionHomeworks = value; }
        }

        #endregion
    }
}
