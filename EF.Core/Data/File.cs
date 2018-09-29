using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class File : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Product> _Products { get; set; }

		[NotMapped]
		public virtual ICollection<Teacher> _Teachers { get; set; }

		[NotMapped]
		public virtual ICollection<Student> _Students { get; set; }

		[NotMapped]
		public virtual ICollection<News> _News { get; set; }

		[NotMapped]
		public virtual ICollection<Message> _Messages { get; set; }

		[NotMapped]
		public virtual ICollection<Blog> _Blogs { get; set; }

		public string Src { get; set; }
		public string Title { get; set; }
		public string Type { get; set; }
		public decimal Size { get; set; }

		#region Navigation Properties
		public virtual ICollection<Product> Products
		{
			get { return _Products ?? (_Products = new List<Product>()); }
			protected set { _Products = value; }
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

		public virtual ICollection<News> News
		{
			get { return _News ?? (_News = new List<News>()); }
			protected set { _News = value; }
		}

		public virtual ICollection<Message> Messages
		{
			get { return _Messages ?? (_Messages = new List<Message>()); }
			protected set { _Messages = value; }
		}

		public virtual ICollection<Blog> Blogs
		{
			get { return _Blogs ?? (_Blogs = new List<Blog>()); }
			protected set { _Blogs = value; }
		}

		#endregion

	}
}
