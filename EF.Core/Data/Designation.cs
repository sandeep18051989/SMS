using System;

namespace EF.Core.Data
{
	public partial class Designation : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}
