using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Subject : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<Teacher> _Teachers { get; set; }
        public string Name { get; set; }
		public Guid SubjectUniqueId { get; set; }
		public string Code { get; set; }
		public string Remarks { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

        public virtual ICollection<Teacher> Teachers
        {
            get { return _Teachers ?? (_Teachers = new List<Teacher>()); }
            protected set { _Teachers = value; }
        }
    }
}
