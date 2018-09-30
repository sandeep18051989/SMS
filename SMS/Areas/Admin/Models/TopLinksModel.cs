using EF.Core.Data;

namespace SMS.Areas.Admin.Models
{
	public class TopLinksModel
	{
		public TopLinksModel() { }

		#region Properties

		public bool IsAuthenticated { get; set; }

		public User user { get; set; }

		public string RoleName { get; set; }
		public int RoleId { get; set; }
		public string UnreadPrivateMessages { get; set; }
		public string AlertMessage { get; set; }

		#endregion
	}
}