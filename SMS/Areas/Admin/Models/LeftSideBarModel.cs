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
        public int LatestNewsCount { get; set; }
        public int LatestBlogsCount { get; set; }
        public int LatestEventsCount { get; set; }
        public int LatestFeedbacksCount { get; set; }
        public int LatestAuditLogsCount { get; set; }
        public int LatestSystemLogsCount { get; set; }

        public int LatestUsersCount { get; set; }
        #endregion

    }
}