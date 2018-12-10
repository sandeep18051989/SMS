using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;
using System.Collections.Generic;

namespace EF.Core.Data
{
	public partial class ClassDivision : BaseEntity
	{
        //[NotMapped]
        //public virtual ICollection<Student> _Students { get; set; }
        public int DivisionId { get; set; }
		public int ClassId { get; set; }
		public int ClassRoomId { get; set; }
		public virtual ClassRoom ClassRoom { get; set; }
        public virtual Division Division { get; set; }
        public virtual Class Class { get; set; }

        //#region Navigation Properties

        //public virtual ICollection<Student> Students
        //{
        //    get { return _Students ?? (_Students = new List<Student>()); }
        //    protected set { _Students = value; }
        //}

        //#endregion
    }
}
