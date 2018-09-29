using System;

namespace EF.Core.Data
{
	public partial class Qualification : BaseEntity
	{
		public string Name { get; set; }
        public string Description { get; set; }
    }
}
