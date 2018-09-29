using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Category : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<Caste> _Castes { get; set; }
		public string CategoryName { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }

		#region Navigation Properties
		public virtual ICollection<Caste> Castes
		{
			get { return _Castes ?? (_Castes = new List<Caste>()); }
			protected set { _Castes = value; }
		}
		#endregion
	}
}
