using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Video : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Blog> _Blogs { get; set; }
		[NotMapped]
		public virtual ICollection<Product> _Products { get; set; }
		[NotMapped]
		public virtual ICollection<Event> _Events { get; set; }
		[NotMapped]
		public virtual ICollection<News> _News { get; set; }
		[NotMapped]
		public virtual ICollection<Reaction> _Reactions { get; set; }
		public string Url { get; set; }
		public decimal Size { get; set; }
		public int DisplayOrder { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }

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
