using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class MessageGroup : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Student> _Students { get; set; }
		[NotMapped]
		public virtual ICollection<Division> _Divisions { get; set; }
		[NotMapped]
		public virtual ICollection<Teacher> _Teachers { get; set; }
		[NotMapped]
		public virtual ICollection<Class> _Classes { get; set; }

		public string GroupName { get; set; }
		public string Description { get; set; }

		#region Navigation Properties

		public virtual ICollection<Division> Divisions
		{
			get { return _Divisions ?? (_Divisions = new List<Division>()); }
			protected set { _Divisions = value; }
		}

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
		public virtual ICollection<Class> Classes
		{
			get { return _Classes ?? (_Classes = new List<Class>()); }
			protected set { _Classes = value; }
		}
		#endregion

	}
}
