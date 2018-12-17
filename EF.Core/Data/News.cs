using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class News : BaseEntity, ISlugSupported
	{
		[NotMapped]
		public virtual ICollection<NewsVideo> _Videos { get; set; }
		[NotMapped]
		public virtual ICollection<NewsPicture> _Pictures { get; set; }
		[NotMapped]
		public virtual ICollection<Comment> _Comments { get; set; }

		[NotMapped]
		public virtual ICollection<File> _Files { get; set; }
		[NotMapped]
		public virtual ICollection<Reaction> _Reactions { get; set; }
		public string ShortName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		public string Author { get; set; }
		public int NewsStatusId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
        public virtual User User { get; set; }

		[NotMapped]
		public NewsStatus NewsStatus
		{
			get
			{
				return (NewsStatus)this.NewsStatusId;
			}
			set
			{
				this.NewsStatusId = (int)value;
			}
		}

		#region Navigation Properties
		public virtual ICollection<NewsVideo> Videos
		{
			get { return _Videos ?? (_Videos = new List<NewsVideo>()); }
			protected set { _Videos = value; }
		}

		public virtual ICollection<NewsPicture> Pictures
		{
			get { return _Pictures ?? (_Pictures = new List<NewsPicture>()); }
			protected set { _Pictures = value; }
		}

		public virtual ICollection<Comment> Comments
		{
			get { return _Comments ?? (_Comments = new List<Comment>()); }
			protected set { _Comments = value; }
		}

		public virtual ICollection<File> Files
		{
			get { return _Files ?? (_Files = new List<File>()); }
			protected set { _Files = value; }
		}

		public virtual ICollection<Reaction> Reactions
		{
			get { return _Reactions ?? (_Reactions = new List<Reaction>()); }
			protected set { Reactions = value; }
		}
		#endregion

	}
}
