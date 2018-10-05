using System;

namespace EF.Core.Data
{
	public partial class NewsVideo : BaseEntity
	{
		public int NewsId { get; set; }
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public virtual News News { get; set; }
		public virtual Video Video { get; set; }
	}
}
