using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Services.Http;
using EF.Services.Service;
using TrackerEnabledDbContext.Common.Interfaces;
using TrackerEnabledDbContext.Common.Models;

namespace EF.Services
{
	/// <summary>
	/// Customer activity service
	/// </summary>
	public class AuditService : IAuditService
	{

		#region Fields
		private readonly ITrackerContext _auditRepository;
		private readonly IUserContext _userContext;
		private readonly IUrlHelper _urlHelper;
		private readonly IUserService _userService;
		#endregion

		#region Ctor

		public AuditService(ITrackerContext auditRepository, IUserContext userContext, IUrlHelper urlHelper, IUserService userService)
		{
			this._auditRepository = auditRepository;
			this._userContext = userContext;
			this._urlHelper = urlHelper;
			this._userService = userService;
		}

		#endregion

		#region Methods

		public virtual IList<AuditLog> GetAllAudits(string entityName)
		{
			if (String.IsNullOrEmpty(entityName))
				throw new Exception("Entity Name Not Specified.");

			return _auditRepository.GetLogs(entityName).ToList();
		}

		public virtual IList<AuditLog> GetAllAuditsByUser(string userid)
		{
			if (String.IsNullOrEmpty(userid))
				throw new Exception("Userid Not Specified.");

			var useraudits = new List<AuditLog>();

			// Product
			var productQuery = _auditRepository.GetLogs<Product>().ToList();
			if (productQuery.Count > 0)
			{
				productQuery = productQuery.Where(x => x.UserName.Trim().ToLower() == userid.Trim().ToLower()).ToList();
			}
			useraudits.AddRange(productQuery);

			// Events
			var eventsQuery = _auditRepository.GetLogs<Event>().ToList();
			if (eventsQuery.Count > 0)
			{
				eventsQuery = eventsQuery.Where(x => x.UserName.Trim().ToLower() == userid.Trim().ToLower()).ToList();
			}
			useraudits.AddRange(eventsQuery);

			// Blogs
			var blogsQuery = _auditRepository.GetLogs<Blog>().ToList();
			if (blogsQuery.Count > 0)
			{
				blogsQuery = blogsQuery.Where(x => x.UserName.Trim().ToLower() == userid.Trim().ToLower()).ToList();
			}
			useraudits.AddRange(blogsQuery);

			return useraudits.OrderByDescending(x=>x.EventDateUTC).ToList();
		}

		public virtual IList<AuditLog> GetAllAuditsByDate(DateTime date)
		{
			if (date == null)
				throw new Exception("Audit date Not Specified.");

			var useraudits = new List<AuditLog>();
			var returnAudits = new List<AuditLog>();

			// Product
			var productQuery = _auditRepository.GetLogs<Product>().ToList();
			if (productQuery.Count > 0)
			{
				productQuery = productQuery.Where(x => x.EventDateUTC == date).ToList();
			}
			useraudits.AddRange(productQuery);

			// Events
			var eventsQuery = _auditRepository.GetLogs<Event>().ToList();
			if (eventsQuery.Count > 0)
			{
				eventsQuery = eventsQuery.Where(x => x.EventDateUTC == date).ToList();
			}
			useraudits.AddRange(eventsQuery);

			// Blogs
			var blogsQuery = _auditRepository.GetLogs<Blog>().ToList();
			if (blogsQuery.Count > 0)
			{
				blogsQuery = blogsQuery.Where(x => x.EventDateUTC == date).ToList();
			}
			useraudits.AddRange(blogsQuery);

			foreach(var aud in useraudits.OrderByDescending(x => x.EventDateUTC).Take(5).ToList())
			{
				 if(aud.EventDateUTC.Date == date.Date)
				{
					returnAudits.Add(aud);
				}
			}

			var audits = returnAudits.ToList();

			return audits;
		}

		#endregion

	}
}
