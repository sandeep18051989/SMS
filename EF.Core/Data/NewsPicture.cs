using System;

namespace EF.Core.Data
{
	public partial class NewsPicture : BaseEntity
	{
		public int NewsId { get; set; }
		public int PictureId { get; set; }
		public bool IsDefault { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public virtual News News { get; set; }
		public virtual Picture Picture { get; set; }

	}
}
