using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Division : BaseEntity
	{

        public string Name { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
        public int DisplayOrder { get; set; }

        #region Navigation Properties

        #endregion
    }
}
