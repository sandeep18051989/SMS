using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Video : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Blog> _Blogs { get; set; }
		[NotMapped]
		public virtual ICollection<ProductVideo> _Products { get; set; }
		[NotMapped]
		public virtual ICollection<EventVideo> _Events { get; set; }
		[NotMapped]
		public virtual ICollection<NewsVideo> _News { get; set; }
		[NotMapped]
		public virtual ICollection<Reaction> _Reactions { get; set; }
		public string Url { get; set; }
		public decimal Size { get; set; }
		public int DisplayOrder { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public string VideoSrc { get; set; }

		#region Navigation Properties
		public virtual ICollection<Blog> Blogs
		{
			get { return _Blogs ?? (_Blogs = new List<Blog>()); }
			protected set { _Blogs = value; }
		}

		public virtual ICollection<ProductVideo> Products
		{
			get { return _Products ?? (_Products = new List<ProductVideo>()); }
			protected set { _Products = value; }
		}

		public virtual ICollection<EventVideo> Events
		{
			get { return _Events ?? (_Events = new List<EventVideo>()); }
			protected set { _Events = value; }
		}

		public virtual ICollection<NewsVideo> News
		{
			get { return _News ?? (_News = new List<NewsVideo>()); }
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
