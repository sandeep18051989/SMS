using System;

namespace EF.Core.Data
{
	public partial class BlogPicture : BaseEntity
	{
		public int BlogId { get; set; }
		public int PictureId { get; set; }
		public bool IsDefault { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public virtual Blog Blog { get; set; }
		public virtual Picture Picture { get; set; }

	}
}
