using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Subject : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Teacher> _Teachers { get; set; }

		[NotMapped]
		public virtual ICollection<DivisionSubject> _ClassDivisionSubjects { get; set; }
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
		public virtual ICollection<DivisionSubject> ClassDivisionSubjects
		{
			get { return _ClassDivisionSubjects ?? (_ClassDivisionSubjects = new List<DivisionSubject>()); }
			protected set { _ClassDivisionSubjects = value; }
		}
	}
}
