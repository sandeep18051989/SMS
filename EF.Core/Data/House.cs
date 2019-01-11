using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class House : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<Student> _Students { get; set; }

        public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public string Description { get; set; }
		public int PictureId { get; set; }
		public virtual Picture Picture { get; set; }
		public int AcadmicYearId { get; set; }

        #region Navigation Properties

        public virtual ICollection<Student> Students
        {
            get { return _Students ?? (_Students = new List<Student>()); }
            protected set { _Students = value; }
        }

        #endregion

    }
}
