using SMS.Models;

namespace SMS.Areas.Admin.Models
{
	public class AdminUserModel
	{
		public AdminUserModel()
		{
			Picture = new AdminPictureModel();
			ChangePassword = new ChangePasswordModel();

		}
		public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Street { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string UserName { get; set; }

		public int CityId { get; set; }

		public string Phone { get; set; }
		public bool IsPhoneVerified { get; set; }
		public bool IsEmailVerified { get; set; }
		public string Hobbies { get; set; }

		public AdminPictureModel Picture { get; set; }

		public ChangePasswordModel ChangePassword { get; set; }

	}
}