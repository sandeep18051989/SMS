using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Caste : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Category> _Categories { get; set; }
		public int ReligionId { get; set; }
		public string Name { get; set; }
		public virtual Religion Religion { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }

		public virtual ICollection<Category> Categories
		{
			get { return _Categories ?? (_Categories = new List<Category>()); }
			protected set { _Categories = value; }
		}

	}
}
