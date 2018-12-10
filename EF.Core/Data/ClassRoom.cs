using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class ClassRoom : BaseEntity
	{
        //[NotMapped]
        //public virtual ICollection<ClassDivision> _Divisions { get; set; }

        //[NotMapped]
        //public virtual ICollection<ClassDivision> _Classes { get; set; }

        public string Number { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

        #region Navigation Properties

        //public virtual ICollection<ClassDivision> Divisions
        //{
        //    get { return _Divisions ?? (_Divisions = new List<ClassDivision>()); }
        //    protected set { _Divisions = value; }
        //}

        //public virtual ICollection<ClassDivision> Classes
        //{
        //    get { return _Classes ?? (_Classes = new List<ClassDivision>()); }
        //    protected set { _Classes = value; }
        //}

        #endregion
    }
}
