using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using SMS.Models;

namespace SMS.Areas.Admin.Models
{
	public class NotificationModel : BaseEntityModel
	{
		public NotificationModel()
		{
			Users = new List<UserModel>();
			AuditRequests = new List<AuditModel>();
			SystemLogs = new List<SystemLogModel>();
			AvailableRoles = new List<SelectListItem>();
		}

		public bool IsAnIssue { get; set; }
		public int SelectedRoleId { get; set; }

		public int NotificationCount { get; set; }

		public IList<UserModel> Users { get; set; }

		public IList<AuditModel> AuditRequests { get; set; }
		public IList<SelectListItem> AvailableRoles { get; set; }

		public IList<SystemLogModel> SystemLogs { get; set; }

	}
}