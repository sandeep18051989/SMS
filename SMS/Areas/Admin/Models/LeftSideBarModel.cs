using SMS.Models;

namespace SMS.Areas.Admin.Models
{
	public partial class LeftSideBarModel
	{
		public LeftSideBarModel()
		{
			Settings = new SettingsViewModel();
			adminUserModel = new AdminUserModel();
		}

		#region Properties

		public AdminUserModel adminUserModel { get; set; }

		public SettingsViewModel Settings { get; set; }

		public string ActiveSettings { get; set; }

		#endregion

	}
}