using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Comment : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Blog> _Blogs { get; set; }
		[NotMapped]
		public virtual ICollection<DivisionExam> _DivisionExams { get; set; }

        [NotMapped]
        public virtual ICollection<StudentExam> _StudentExams { get; set; }

        [NotMapped]
        public virtual ICollection<TeacherExam> _TeacherExams { get; set; }

        [NotMapped]
        public virtual ICollection<StudentHomework> _StudentHomeworks { get; set; }
        [NotMapped]
        public virtual ICollection<DivisionHomework> _DivisionHomeworks { get; set; }

        [NotMapped]
		public virtual ICollection<Product> _Products { get; set; }

		[NotMapped]
		public virtual ICollection<Event> _Events { get; set; }

		[NotMapped]
		public virtual ICollection<News> _News { get; set; }

		[NotMapped]
		public virtual ICollection<Homework> _Homeworks { get; set; }
		[NotMapped]
		public virtual ICollection<Reaction> _Reactions { get; set; }

		public int DisplayOrder { get; set; }
		public bool IsApproved { get; set; }
		public string BlockReason { get; set; }
		public int BlockedBy { get; set; }
		public string CommentHtml { get; set; }

		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public string Username { get; set; }

		#region Navigation Properties

		public virtual ICollection<Blog> Blogs
		{
			get { return _Blogs ?? (_Blogs = new List<Blog>()); }
			protected set { _Blogs = value; }
		}

		public virtual ICollection<Product> Products
		{
			get { return _Products ?? (_Products = new List<Product>()); }
			protected set { _Products = value; }
		}

		public virtual ICollection<Event> Events
		{
			get { return _Events ?? (_Events = new List<Event>()); }
			protected set { _Events = value; }
		}

		public virtual ICollection<DivisionExam> DivisionExams
		{
			get { return _DivisionExams ?? (_DivisionExams = new List<DivisionExam>()); }
			protected set { _DivisionExams = value; }
		}
        public virtual ICollection<StudentExam> StudentExams
        {
            get { return _StudentExams ?? (_StudentExams = new List<StudentExam>()); }
            protected set { _StudentExams = value; }
        }
        public virtual ICollection<TeacherExam> TeacherExams
        {
            get { return _TeacherExams ?? (_TeacherExams = new List<TeacherExam>()); }
            protected set { _TeacherExams = value; }
        }

        public virtual ICollection<StudentHomework> StudentHomeworks
        {
            get { return _StudentHomeworks ?? (_StudentHomeworks = new List<StudentHomework>()); }
            protected set { _StudentHomeworks = value; }
        }

        public virtual ICollection<DivisionHomework> DivisionHomeworks
        {
            get { return _DivisionHomeworks ?? (_DivisionHomeworks = new List<DivisionHomework>()); }
            protected set { _DivisionHomeworks = value; }
        }

        public virtual ICollection<News> News
		{
			get { return _News ?? (_News = new List<News>()); }
			protected set { _News = value; }
		}
		public virtual ICollection<Reaction> Reactions
		{
			get { return _Reactions ?? (_Reactions = new List<Reaction>()); }
			protected set { Reactions = value; }
		}

		#endregion

	}
}
