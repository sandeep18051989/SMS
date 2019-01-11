using System.ComponentModel.DataAnnotations;

namespace EF.Core.Data
{
	public partial class AcadmicYear : BaseEntity
	{
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public virtual User User { get; set; }
	}
}
