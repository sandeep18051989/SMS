using System;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;
using System.Collections.Generic;

namespace EF.Core.Data
{
	public partial class ClassRoomDivision : BaseEntity
	{
        //[NotMapped]
        //public virtual ICollection<Student> _Students { get; set; }

        [NotMapped]
        public virtual ICollection<Teacher> _Teachers { get; set; }

        [NotMapped]
        public virtual ICollection<MessageGroup> _MessageGroups { get; set; }


        public int? ClassRoomId { get; set; }
		public virtual ClassRoom ClassRoom { get; set; }
        public int? ClassId { get; set; }
        public virtual Class Class { get; set; }
        public int? DivisionId { get; set; }
        public virtual Division Division { get; set; }

        #region Navigation Properties

        public virtual ICollection<Teacher> Teachers
        {
            get { return _Teachers ?? (_Teachers = new List<Teacher>()); }
            protected set { _Teachers = value; }
        }

        //public virtual ICollection<Student> Students
        //{
        //    get { return _Students ?? (_Students = new List<Student>()); }
        //    protected set { _Students = value; }
        //}

        public virtual ICollection<MessageGroup> MessageGroups
        {
            get { return _MessageGroups ?? (_MessageGroups = new List<MessageGroup>()); }
            protected set { _MessageGroups = value; }
        }

        #endregion
    }
}
