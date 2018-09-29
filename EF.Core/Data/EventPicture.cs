using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class EventPicture : BaseEntity
	{
		public int EventId { get; set; }
		public int PictureId { get; set; }
		public bool IsDefault { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public virtual Event Event { get; set; }
		public virtual Picture Picture { get; set; }
	}
}
