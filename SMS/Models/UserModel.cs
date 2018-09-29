using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EF.Services;

namespace SMS.Models
{

	public partial class UserModel : BaseEntityModel
	{
		public UserModel()
		{
			ChangePassword = new ChangePasswordModel();
			AvailableRoles = new List<RoleModel>();
		}

		public string Username { get; set; }
		[EmailAddress]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		public int[] SelectedRoleIds { get; set; }
		public bool IsApproved { get; set; }
		public bool IsActive { get; set; }
		public bool IsBlocked { get; set; }
		public Guid UserGuid { get; set; }

		public ChangePasswordModel ChangePassword { get; set; }

		public IList<RoleModel> AvailableRoles { get; set; }

	}
}