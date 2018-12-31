using System;

namespace EF.Core.Data
{
	public partial class BlogVideo : BaseEntity
	{
		public int BlogId { get; set; }
		public int VideoId { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public virtual Blog Blog { get; set; }
		public virtual Video Video { get; set; }
	}
}
