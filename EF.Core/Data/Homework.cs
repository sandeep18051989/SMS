using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Homework : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<DivisionHomework> _DivisionHomeworks { get; set; }
        [NotMapped]
        public virtual ICollection<StudentHomework> _StudentHomeworks { get; set; }

        public string Name { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

        public virtual ICollection<DivisionHomework> DivisionHomeworks
        {
            get { return _DivisionHomeworks ?? (_DivisionHomeworks = new List<DivisionHomework>()); }
            protected set { _DivisionHomeworks = value; }
        }

        public virtual ICollection<StudentHomework> StudentHomeworks
        {
            get { return _StudentHomeworks ?? (_StudentHomeworks = new List<StudentHomework>()); }
            protected set { _StudentHomeworks = value; }
        }

    }
}
