using System;

namespace EF.Core.Data
{
	public partial class ProductPicture : BaseEntity
	{
		public int ProductId { get; set; }
		public int PictureId { get; set; }
		public bool IsDefault { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public virtual Product Product { get; set; }
		public virtual Picture Picture { get; set; }

	}
}
