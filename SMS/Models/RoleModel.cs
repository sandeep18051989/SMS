using EF.Core.Data;
using EF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
	public partial class RoleModel : BaseEntityModel
	{
		public string RoleName { get; set; }
		public bool IsSystemDefined { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		public int AcadmicYearId { get; set; }
		public virtual AcadmicYear AcadmicYear { get; set; }
	}
}