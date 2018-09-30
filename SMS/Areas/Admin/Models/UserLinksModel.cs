using System.Web.Mvc;
using EF.Core.Data;

namespace SMS.Areas.Admin.Models
{
	public class UserLinksModel
	{
		public UserLinksModel() { }

		#region Properties

		public bool IsAuthenticated { get; set; }

		[AllowHtml]
		public string Links { get; set; }

		public User user { get; set; }

        public string CustomerUsername { get; set; }

        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string UnreadPrivateMessages { get; set; }
        public string AlertMessage { get; set; }

        #endregion
    }
}