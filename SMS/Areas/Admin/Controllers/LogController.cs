using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Service;
using SMS.Areas.Admin.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class LogController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IUserContext _userContext;
		private readonly ISettingService _settingService;
		private readonly IPermissionService _permissionService;
		private readonly ISystemLogService _systemLogService;

		#endregion Fileds

		#region Constructor

		public LogController(IUserService userService, IUserContext userContext, ISettingService settingService, IPermissionService permissionService, ISystemLogService systemLogService)
		{
			this._userService = userService;
			this._userContext = userContext;
			this._settingService = settingService;
			this._permissionService = permissionService;
			this._systemLogService = systemLogService;
		}

		#endregion
		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageLogs"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			var model = new List<SystemLogModel>();
			var lstSystemLogs = _systemLogService.GetAllSystemLogs().OrderByDescending(x => x.CreatedOn).ToList();
			if (lstSystemLogs.Count > 0)
			{
				foreach (var eve in lstSystemLogs)
				{
					model.Add(new SystemLogModel
					{
						Id = eve.Id,
						EntityId = eve.EntityId,
						EntityType = eve.EntityType,
						EntityTypeName = eve.EntityTypeName,
						ErrorId = eve.ErrorId,
						IpAddress = eve.IpAddress,
						IsException = eve.IsException,
						IsFixed = eve.IsFixed,
						Level = eve.Level,
						LogLevel = eve.LogLevel,
						Message = eve.Message,
						StackTrace = eve.StackTrace,
						Url = eve.Url,
						Date = eve.CreatedOn
					});
				}
			}

			return View(model.ToList());
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageLogs"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			var _logEntry = _systemLogService.GetSystemLogById(id);

			if (_logEntry != null)
				_systemLogService.DeleteLog(_logEntry);

			SuccessNotification("System Log Entry Deleted Successfully.");
			return RedirectToAction("List");
		}

		[HttpPost]
		public ActionResult DeleteSelected(ICollection<int> selectedIds)
		{
			if (!_permissionService.Authorize("ManageLogs"))
				return AccessDeniedView();

			if (selectedIds != null)
			{
				_systemLogService.DeleteLogs(_systemLogService.GetSystemLogByIds(selectedIds.ToArray()).ToList());
			}


			SuccessNotification("System Log Entries Deleted Successfully.");
			return RedirectToAction("List");
		}

	}
}
