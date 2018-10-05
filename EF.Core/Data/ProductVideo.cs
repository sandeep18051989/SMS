using System;

namespace EF.Core.Data
{
	public partial class ProductVideo : BaseEntity
	{
		public int ProductId { get; set; }
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public virtual Product Product { get; set; }
		public virtual Video Video { get; set; }
	}
}
