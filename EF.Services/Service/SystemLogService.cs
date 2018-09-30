using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Http;

namespace EF.Services.Service
{
	public class SystemLogService : ISystemLogService
	{
		#region Fields

		private readonly IRepository<SystemLog> _systemlogRepository;
		private readonly IUrlHelper _urlHelper;
		private readonly IEmailService _emailService;

		#endregion

		#region Const

		public SystemLogService(IRepository<SystemLog> logRepository, IUrlHelper urlHelper, IEmailService emailService)
		{
			this._systemlogRepository = logRepository;
			this._urlHelper = urlHelper;
			this._emailService = emailService;
		}

		#endregion

		#region Utilities

		#endregion

		#region Methods

		/// <summary>
		/// Deletes a systemlog item
		/// </summary>
		/// <param name="systemlog">SystemLog item</param>
		public virtual void DeleteLog(SystemLog systemlog)
		{
			if (systemlog == null)
				throw new ArgumentNullException("systemlog");

			_systemlogRepository.Delete(systemlog);
		}

		/// <summary>
		/// Deletes a systemlog items
		/// </summary>
		/// <param name="systemlogs">SystemLog items</param>
		public virtual void DeleteLogs(IList<SystemLog> systemlogs)
		{
			if (systemlogs == null)
				throw new ArgumentNullException("systemlogs");

			foreach (SystemLog syslog in systemlogs)
				_systemlogRepository.Delete(syslog);
		}

		/// <summary>
		/// Clears a systemlog
		/// </summary>
		public virtual void ClearLog()
		{
			var systemlog = _systemlogRepository.Table.ToList();
			foreach (var systemlogItem in systemlog)
				_systemlogRepository.Delete(systemlogItem);
		}

		/// <summary>
		/// Gets all systemlog items
		/// </summary>
		/// <param name="fromUtc">SystemLog item creation from; null to load all records</param>
		/// <param name="toUtc">SystemLog item creation to; null to load all records</param>
		/// <param name="message">Message</param>
		/// <param name="logLevel">SystemLog level; null to load all records</param>
		/// <returns>SystemLog item items</returns>
		public virtual IList<SystemLog> GetAllSystemLogs(DateTime? fromUtc = null, DateTime? toUtc = null,
			 string message = "", LogLevel? Level = null, bool? isFixed = null)
		{
			var lstReturn = new List<SystemLog>();
			var query = _systemlogRepository.Table;
			
			if (toUtc.HasValue)
				query = query.Where(l => toUtc.Value >= l.CreatedOn);

			if (isFixed.HasValue)
				query = query.Where(l => l.IsFixed);

			if (Level.HasValue)
			{
				var systemlogLevelId = (int)Level.Value;
				query = query.Where(l => systemlogLevelId == l.Level);
			}
			if (!String.IsNullOrEmpty(message))
				query = query.Where(l => l.Message.Contains(message) || l.StackTrace.Contains(message));
			query = query.OrderByDescending(l => l.CreatedOn);

			if (fromUtc.HasValue)
			{
				foreach (var aud in query.OrderByDescending(x => x.CreatedOn).Take(5).ToList())
				{
					if (aud.CreatedOn.Date == fromUtc.Value.Date)
					{
						lstReturn.Add(aud);
					}
				}

				return lstReturn;
			}

			return query.ToList();
		}

		/// <summary>
		/// Gets a systemlog item
		/// </summary>
		/// <param name="systemlogId">SystemLog item identifier</param>
		/// <returns>SystemLog item</returns>
		public virtual SystemLog GetSystemLogById(int systemlogId)
		{
			if (systemlogId == 0)
				return null;

			return _systemlogRepository.GetByID(systemlogId);
		}

		/// <summary>
		/// Get systemlog items by identifiers
		/// </summary>
		/// <param name="systemlogIds">SystemLog item identifiers</param>
		/// <returns>SystemLog items</returns>
		public virtual IList<SystemLog> GetSystemLogByIds(int[] systemlogIds)
		{
			if (systemlogIds == null || systemlogIds.Length == 0)
				return new List<SystemLog>();

			var query = from l in _systemlogRepository.Table
							where systemlogIds.Contains(l.Id)
							select l;
			var systemlogItems = query.ToList();
			//sort by passed identifiers
			var sortedSystemLogItems = new List<SystemLog>();
			foreach (int id in systemlogIds)
			{
				var systemlog = systemlogItems.Find(x => x.Id == id);
				if (systemlog != null)
					sortedSystemLogItems.Add(systemlog);
			}
			return sortedSystemLogItems;
		}

		/// <summary>
		/// Inserts a systemlog item
		/// </summary>
		/// <param name="systemlogLevel">SystemLog level</param>
		/// <param name="shortMessage">The short message</param>
		/// <param name="fullMessage">The full message</param>
		/// <param name="customer">The customer to associate systemlog record with</param>
		/// <returns>A systemlog item</returns>
		public virtual SystemLog InsertSystemLog(LogLevel Level, string Message, string StackTrace = "", User user = null)
		{
			var systemlog = new SystemLog
			{
				LogLevel = Level,
				Message = Message,
				StackTrace = StackTrace,
				IpAddress = _urlHelper.GetCurrentIpAddress(),
				UserId = user != null ? user.Id : 0,
				Url = _urlHelper.GetThisPageUrl(true),
				CreatedOn = DateTime.UtcNow,
				ModifiedOn = DateTime.Now,
				IsFixed = false
			};

			_systemlogRepository.Insert(systemlog);

			// Send Mail To Admin
			_emailService.SendMail("sandeep725@gmail.com","Artery Labs Exception Log -" + systemlog.CreatedOn, !String.IsNullOrEmpty(Message) ? Message : "");


			return systemlog;
		}

		#endregion
	}
}
