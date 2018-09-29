using EF.Core.Data;
using System;
using System.Collections.Generic;
using TrackerEnabledDbContext.Common.Models;

namespace EF.Services
{
	/// <summary>
	/// Customer activity service interface
	/// </summary>
	public partial interface IAuditService
	{
		IList<AuditLog> GetAllAudits(string entityName);
		IList<AuditLog> GetAllAuditsByUser(string userid);
		IList<AuditLog> GetAllAuditsByDate(DateTime date);
	}
}
