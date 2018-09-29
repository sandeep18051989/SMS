using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class Homework : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		private int StudentApprovalStatusId { get; set; }
		private int TeacherApprovalStatusId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }

		[NotMapped]
		public virtual ICollection<Comment> _Comments { get; set; }
		[NotMapped]
		public StudentHomeWorkStatus StudentHomeWorkStatus
		{
			get
			{
				return (StudentHomeWorkStatus)this.StudentApprovalStatusId;
			}
			set
			{
				this.StudentApprovalStatusId = (int)value;
			}
		}
		[NotMapped]
		public TeacherApprovalStatus TeacherApprovalStatus
		{
			get
			{
				return (TeacherApprovalStatus)this.TeacherApprovalStatusId;
			}
			set
			{
				this.TeacherApprovalStatusId = (int)value;
			}
		}

		public virtual ICollection<Comment> Comments
		{
			get { return _Comments ?? (_Comments = new List<Comment>()); }
			protected set { _Comments = value; }
		}
	}
}
