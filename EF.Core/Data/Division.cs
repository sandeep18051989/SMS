using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Division : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<MessageGroup> _MessageGroups { get; set; }

        [NotMapped]
        public virtual ICollection<ClassDivision> _Classes { get; set; }

        public string Name { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
        public int DisplayOrder { get; set; }

        #region Navigation Properties

        public virtual ICollection<ClassDivision> Classes
        {
            get { return _Classes ?? (_Classes = new List<ClassDivision>()); }
            protected set { _Classes = value; }
        }

        public virtual ICollection<MessageGroup> MessageGroups
		{
			get { return _MessageGroups ?? (_MessageGroups = new List<MessageGroup>()); }
			protected set { _MessageGroups = value; }
		}

		#endregion

	}
}
