using System.Collections.Generic;
using System.Web.Mvc;
using TrackerEnabledDbContext.Common.Models;

namespace SMS.Areas.Admin.Models
{
	public class AuditModelListModel
    {
        public AuditModelListModel()
        {
            AuditLogs = new List<AuditLog>();
            Entities = new List<SelectListItem>();
        }
        public IList<AuditLog> AuditLogs { get; set; }
        public IList<SelectListItem> Entities { get; set; }
        public string SelectedEntityName { get; set; }

    }
}