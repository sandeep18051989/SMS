using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class MessageGroup : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Student> _Students { get; set; }
		[NotMapped]
		public virtual ICollection<Teacher> _Teachers { get; set; }

        [NotMapped]
        public virtual ICollection<ClassRoomDivision> _ClassRoomDivisions { get; set; }

        public string Name { get; set; }
		public string Description { get; set; }

		#region Navigation Properties

		public virtual ICollection<Teacher> Teachers
		{
			get { return _Teachers ?? (_Teachers = new List<Teacher>()); }
			protected set { _Teachers = value; }
		}
		public virtual ICollection<Student> Students
		{
			get { return _Students ?? (_Students = new List<Student>()); }
			protected set { _Students = value; }
		}
        public virtual ICollection<ClassRoomDivision> ClassRoomDivisions
        {
            get { return _ClassRoomDivisions ?? (_ClassRoomDivisions = new List<ClassRoomDivision>()); }
            protected set { _ClassRoomDivisions = value; }
        }

        #endregion

    }
}
