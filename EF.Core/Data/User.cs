using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class User : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<UserRole> _UserRoles { get; set; }
		[NotMapped]
		public virtual ICollection<News> _News { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public bool IsApproved { get; set; }
		public Guid UserGuid { get; set; }
		public string SeoName { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }

		#region Navigation Properties
		public virtual ICollection<UserRole> Roles
		{
			get { return _UserRoles ?? (_UserRoles = new List<UserRole>()); }
			protected set { _UserRoles = value; }
		}
		public virtual ICollection<News> News
		{
			get { return _News ?? (_News = new List<News>()); }
			protected set { _News = value; }
		}
		#endregion

	}
}
