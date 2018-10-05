using System;

namespace EF.Core.Data
{
	public partial class EventVideo : BaseEntity
	{
		public int EventId { get; set; }
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public virtual Event Event { get; set; }
		public virtual Video Video { get; set; }
	}
}
