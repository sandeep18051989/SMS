using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Data
{
	public partial class UserRole : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<PermissionRecord> _PermissionRecords { get; set; }
		public string RoleName { get; set; }
		public bool IsSystemDefined { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }

		public virtual ICollection<PermissionRecord> PermissionRecords
		{
			get { return _PermissionRecords ?? (_PermissionRecords = new List<PermissionRecord>()); }
			protected set { _PermissionRecords = value; }
		}

	}
}
