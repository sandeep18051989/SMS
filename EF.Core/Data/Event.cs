using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Event : BaseEntity, ISlugSupported
	{
		[NotMapped]
		public virtual ICollection<Video> _Videos { get; set; }
		[NotMapped]
		public virtual ICollection<Comment> _Comments { get; set; }
		[NotMapped]
		public virtual ICollection<Reaction> _Reactions { get; set; }
		[NotMapped]
		public virtual ICollection<EventPicture> _Pictures { get; set; }
		public string Title { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Description { get; set; }
		public bool IsClosed { get; set; }
		public bool IsApproved { get; set; }
		public string Venue { get; set; }

		public string SeoName { get; set; }

		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

		#region Navigation Properties
		public virtual ICollection<Video> Videos
		{
			get { return _Videos ?? (_Videos = new List<Video>()); }
			protected set { _Videos = value; }
		}

		public virtual ICollection<EventPicture> Pictures
		{
			get { return _Pictures ?? (_Pictures = new List<EventPicture>()); }
			protected set { _Pictures = value; }
		}

		public virtual ICollection<Comment> Comments
		{
			get { return _Comments ?? (_Comments = new List<Comment>()); }
			protected set { _Comments = value; }
		}
		public virtual ICollection<Reaction> Reactions
		{
			get { return _Reactions ?? (_Reactions = new List<Reaction>()); }
			protected set { Reactions = value; }
		}
		#endregion

	}
}
