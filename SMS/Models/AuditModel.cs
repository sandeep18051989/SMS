using System;
using System.Collections.Generic;
using TrackerEnabledDbContext.Common.Models;

namespace SMS.Models
{
	public class AuditModel
	{
		public AuditModel()
		{
			LogDetails = new List<AuditLogDetail>();
			Metadata = new List<LogMetadata>();
		}
		public long AuditLogId { get; set; }
		public DateTime EventDateUTC { get; set; }
		public EventType EventType { get; set; }
		public IList<AuditLogDetail> LogDetails { get; set; }
		public IList<LogMetadata> Metadata { get; set; }

		public string RecordId { get; set; }

		public string EntityName { get; set; }
		public string UserName { get; set; }
		public string TypeFullName { get; set; }

	}
}