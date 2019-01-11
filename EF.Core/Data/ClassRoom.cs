using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class ClassRoom : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<DivisionExam> _DivisionExams { get; set; }

        public string Number { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

        #region Navigation Properties

        public virtual ICollection<DivisionExam> DivisionExams
        {
            get { return _DivisionExams ?? (_DivisionExams = new List<DivisionExam>()); }
            protected set { _DivisionExams = value; }
        }


        #endregion
    }
}
