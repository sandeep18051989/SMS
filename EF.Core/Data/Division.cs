using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Division : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<MessageGroup> _MessageGroups { get; set; }

		public int ClassId { get; set; }
		public string DivisionName { get; set; }
		public string Description { get; set; }
		public virtual Class Class { get; set; }
		public int ClassRoomId { get; set; }
		public virtual ClassRoom ClassRoom { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		#region Navigation Properties

		public virtual ICollection<MessageGroup> MessageGroups
		{
			get { return _MessageGroups ?? (_MessageGroups = new List<MessageGroup>()); }
			protected set { _MessageGroups = value; }
		}

		#endregion

	}
}
