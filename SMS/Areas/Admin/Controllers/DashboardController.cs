using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services;
using EF.Services.Culture;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class DashboardController : AdminAreaController
	{
		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly IAuthenticationService _authenticationService;
		private readonly IRoleService _roleService;
		private readonly IPermissionService _permissionService;
		private readonly ICultureHelper _cultureHelper;
		private readonly IFeedbackService _feedbackService;
		private readonly IEventService _eventService;
		private readonly ICommentService _commentService;
		private readonly ISystemLogService _systemLogService;
		private readonly IAuditService _auditService;
		private readonly IReplyService _replyService;
		private readonly ISMSService _smsService;

		#endregion Fileds

		#region Const

		public DashboardController(IUserService userService, IPictureService pictureService, IUserContext userContext, IAuthenticationService authenticationService, IRoleService roleService, IPermissionService permissionService, ICultureHelper cultureHelper, IFeedbackService feedbackService, IEventService eventService, ICommentService commentService, IAuditService auditService, ISystemLogService systemLogService, IReplyService replyService, ISMSService smsService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._authenticationService = authenticationService;
			this._roleService = roleService;
			this._permissionService = permissionService;
			this._cultureHelper = cultureHelper;
			this._feedbackService = feedbackService;
			this._eventService = eventService;
			this._commentService = commentService;
			this._auditService = auditService;
			this._systemLogService = systemLogService;
			this._replyService = replyService;
			this._smsService = smsService;
		}

		#endregion
		public ActionResult Index()
		{
			if (!_permissionService.Authorize("ManageDashboard"))
				return AccessDeniedView();

			var model = new DashboardModel();
			prepareRegisteredUsers(model);
			prepareFeedbacks(model);
			prepareActiveInactiveUsers(model);
			prepareEvents(model);

			model.ActiveSettings = "";
			model.VisitsToday = _userService.GetUserCountByLoginDate(DateTime.Now);
			model.EventsToday = _eventService.GetEventCountByCreatedDate(DateTime.Now);
			model.CommentsToday = _commentService.GetCommentCountByCreatedDate(DateTime.Now);
			model.FeedbacksToday = _feedbackService.GetFeedbackCountByCreatedDate(DateTime.Now);
			//model.ReturnUsers = _userService.GetAllUserLocations().GroupBy(j => j.UserId).Where(g => g.Count() > 1 && g.FirstOrDefault().UserId != 0).Select(u => new IPAddress() {
			//	Address = u.FirstOrDefault().Address,
			//	CreatedOn = u.FirstOrDefault().CreatedOn,
			//	Location = u.FirstOrDefault().Location.Trim().Replace(",", ""),
			//	Latitude = u.FirstOrDefault().Latitude,
			//	Longitude = u.FirstOrDefault().Longitude,
			//	Id = u.FirstOrDefault().Id,
			//	UserId = u.FirstOrDefault().UserId
			//}).ToList();

			//model.UniqueUsers = _userService.GetAllUserLocations().GroupBy(j => j.UserId).Where(g => g.Count() == 1 || g.FirstOrDefault().UserId == 0).Select(u => new IPAddress()
			//{
			//	Address = u.FirstOrDefault().Address,
			//	CreatedOn = u.FirstOrDefault().CreatedOn,
			//	Location = u.FirstOrDefault().Location.Trim().Replace(",",""),
			//	Latitude = u.FirstOrDefault().Latitude,
			//	Longitude = u.FirstOrDefault().Longitude,
			//	Id = u.FirstOrDefault().Id,
			//	UserId = u.FirstOrDefault().UserId
			//}).ToList();

			//var lstAddresses = new List<IPAddress>();
			//lstAddresses.AddRange(model.ReturnUsers);
			//lstAddresses.AddRange(model.UniqueUsers);
			//model.ConsolidateUserModel = lstAddresses.GroupBy(mu => mu.CreatedOn.Date).Select(mu => new ConsolidateUserModel() {
			//	ReturnCount = mu.Intersect(model.ReturnUsers).Count(),
			//	UniqueCount = mu.Intersect(model.UniqueUsers).Count(),
			//	Date = mu.Key,
			//}).ToList();

			model.Comments = _commentService.GetCommentsByManualDate(DateTime.Now).Select(c => new AdminCommentsModel()
			{
				CommentDate = c.CreatedOn,
				CommentHtml = c.CommentHtml,
				DisplayOrder = c.DisplayOrder,
				UserId = c.UserId,
				UserName = c.Username,
				Replies = _replyService.GetAllRepliesByComment(c.Id).Select(r => new ReplyModel() {
					DisplayOrder = r.DisplayOrder,
					Id = r.Id,
					CommentId = r.CommentId,
					CreatedOn = r.CreatedOn,
					ReplyHtml = r.ReplyHtml,
					UserId = r.UserId,
				}).OrderBy(r => r.DisplayOrder).ToList()
			}).OrderByDescending(c => c.CommentDate).ToList();

			//model.MergedUsers = lstAddresses;
			return View(model);
		}

		#region User Registrations			  
		public DashboardModel prepareRegisteredUsers(DashboardModel model)
		{
			var _totalWeeks = _cultureHelper.GetTotalWeeksInAMonth(model.SelectedYear > 0 ? model.SelectedYear : DateTime.Now.Year, model.SelectedMonth > 0 ? model.SelectedMonth == 1 ? model.SelectedMonth : model.SelectedMonth : DateTime.Now.Month);

			model.SelectedMonth = model.SelectedMonth > 0 ? model.SelectedMonth : DateTime.Now.Month > 1 ? DateTime.Now.Month - 1 : DateTime.Now.Month;
			model.SelectedYear = model.SelectedYear > 0 ? model.SelectedYear : DateTime.Now.Year;

			if (_totalWeeks.Count > 0)
			{
				var _regUsers = _userService.GetAllUsers(false).Where(x => x.Id != 1);

				foreach (var weekGroup in _totalWeeks)
				{
					var _chartModel = new DashboardModel.RegisteredUsersChartModel();
					_chartModel.WeekStart = Convert.ToDateTime(weekGroup.Item2).Date;
					_chartModel.WeekEnd = Convert.ToDateTime(weekGroup.Item3).Date;
					var q = (from x in _regUsers
								where x.CreatedOn.Date >= _chartModel.WeekStart.Date && x.CreatedOn.Date <= _chartModel.WeekEnd.Date
								select new { x.Id, x.UserName }).ToList();

					_chartModel.RegisteredUsers = q.AsEnumerable().Select(item => new KeyValuePair<int, string>(item.Id, item.UserName)).ToList();

					model.RegisteredUsersChartModels.Add(_chartModel);
				}
			}

			// Load Available Months
			var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.TakeWhile(m => m != String.Empty).Select((m, i) => new { Month = i < 9 ? Convert.ToString((i + 1)) : Convert.ToString(i + 1), MonthName = m }).ToList();
			model.AvailableMonths.Add(new SelectListItem { Text = "-- Select Month --", Value = "0" });
			foreach (var t in months)
				model.AvailableMonths.Add(new SelectListItem { Text = t.MonthName, Value = t.Month.ToString() });

			if (model.SelectedMonth > 0)
			{
				var _MonthToBeSelected = model.AvailableMonths.FirstOrDefault(ss => ss.Value == model.SelectedMonth.ToString().PadLeft(2, '0'));
				if (_MonthToBeSelected != null)
				{
					model.AvailableMonths.ToList().ForEach(u => u.Selected = false);
					model.AvailableMonths.FirstOrDefault(ss => ss.Value == model.SelectedMonth.ToString().PadLeft(2, '0')).Selected = true;
				}
			}

			// Load Available Years
			var years = Enumerable.Range(DateTime.Now.Year - 2, 3).OrderByDescending(x => x).ToList();
			model.AvailableYears.Add(new SelectListItem { Text = "-- Select Year --", Value = "0" });
			foreach (var t in years)
				model.AvailableYears.Add(new SelectListItem { Text = t.ToString(), Value = t.ToString() });

			if (model.SelectedYear > 0)
			{
				var _YearToBeSelected = model.AvailableYears.FirstOrDefault(ss => ss.Value == model.SelectedYear.ToString());
				if (_YearToBeSelected != null)
				{
					model.AvailableYears.ToList().ForEach(u => u.Selected = false);
					model.AvailableYears.FirstOrDefault(ss => ss.Value == model.SelectedYear.ToString()).Selected = true;
				}
			}

			return model;
		}

		public ActionResult DrawRegisteredUsersChart(int month, int year)
		{
			if (month == 0 && year == 0)
				throw new Exception("Invalid Month And Year Selected!");

			ModelState.Clear();
			var model = new DashboardModel();
			model.SelectedMonth = month;
			model.SelectedYear = year;
			prepareRegisteredUsers(model);
			return PartialView("~/Areas/Admin/Views/Charts/_RegisteredUsers.cshtml", model);
		}

		#endregion

		#region Registered Feedbacks

		public DashboardModel prepareFeedbacks(DashboardModel model)
		{
			var _totalWeeks = _cultureHelper.GetTotalWeeksInAMonth(model.Feedbacks_SelectedYear > 0 ? model.Feedbacks_SelectedYear : DateTime.Now.Year, model.Feedbacks_SelectedMonth > 0 ? model.Feedbacks_SelectedMonth == 1 ? model.Feedbacks_SelectedMonth : model.Feedbacks_SelectedMonth : DateTime.Now.Month);

			model.Feedbacks_SelectedMonth = model.Feedbacks_SelectedMonth > 0 ? model.Feedbacks_SelectedMonth : DateTime.Now.Month > 1 ? DateTime.Now.Month - 1 : DateTime.Now.Month;
			model.Feedbacks_SelectedYear = model.Feedbacks_SelectedYear > 0 ? model.Feedbacks_SelectedYear : DateTime.Now.Year;

			if (_totalWeeks.Count > 0)
			{
				var _feedbacks = _feedbackService.GetFeedbacks().ToList();

				foreach (var weekGroup in _totalWeeks)
				{
					var _chartModel = new DashboardModel.RegisteredFeedbacks();
					_chartModel.Feedbacks_WeekStart = Convert.ToDateTime(weekGroup.Item2).Date;
					_chartModel.Feedbacks_WeekEnd = Convert.ToDateTime(weekGroup.Item3).Date;
					var q = (from x in _feedbacks
								where x.CreatedOn.Date >= _chartModel.Feedbacks_WeekStart.Date && x.CreatedOn.Date <= _chartModel.Feedbacks_WeekEnd.Date
								select new { x.Id, x.Email }).ToList();

					_chartModel.Feedbacks = q.AsEnumerable().Select(item => new KeyValuePair<int, string>(item.Id, item.Email)).ToList();

					model.FeedbacksChartModels.Add(_chartModel);
				}
			}

			// Load Available Months
			var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.TakeWhile(m => m != String.Empty).Select((m, i) => new { Month = i < 9 ? Convert.ToString((i + 1)) : Convert.ToString(i + 1), MonthName = m }).ToList();
			model.Feedbacks_AvailableMonths.Add(new SelectListItem { Text = "-- Select Month --", Value = "0" });
			foreach (var t in months)
				model.Feedbacks_AvailableMonths.Add(new SelectListItem { Text = t.MonthName, Value = t.Month.ToString() });

			if (model.Feedbacks_SelectedMonth > 0)
			{
				var _MonthToBeSelected = model.Feedbacks_AvailableMonths.FirstOrDefault(ss => ss.Value == model.Feedbacks_SelectedMonth.ToString().PadLeft(2, '0'));
				if (_MonthToBeSelected != null)
				{
					model.Feedbacks_AvailableMonths.ToList().ForEach(u => u.Selected = false);
					model.Feedbacks_AvailableMonths.FirstOrDefault(ss => ss.Value == model.Feedbacks_SelectedMonth.ToString().PadLeft(2, '0')).Selected = true;
				}
			}

			// Load Available Years
			var years = Enumerable.Range(DateTime.Now.Year - 2, 3).OrderByDescending(x => x).ToList();
			model.Feedbacks_AvailableYears.Add(new SelectListItem { Text = "-- Select Year --", Value = "0" });
			foreach (var t in years)
				model.Feedbacks_AvailableYears.Add(new SelectListItem { Text = t.ToString(), Value = t.ToString() });

			if (model.Feedbacks_SelectedYear > 0)
			{
				var _YearToBeSelected = model.Feedbacks_AvailableYears.FirstOrDefault(ss => ss.Value == model.Feedbacks_SelectedYear.ToString());
				if (_YearToBeSelected != null)
				{
					model.Feedbacks_AvailableYears.ToList().ForEach(u => u.Selected = false);
					model.Feedbacks_AvailableYears.FirstOrDefault(ss => ss.Value == model.Feedbacks_SelectedYear.ToString()).Selected = true;
				}
			}

			return model;
		}

		public ActionResult DrawFeedbacksChart(int month, int year)
		{
			if (month == 0 && year == 0)
				throw new Exception("Invalid Month And Year Selected!");

			ModelState.Clear();
			var model = new DashboardModel();
			model.Feedbacks_SelectedMonth = month;
			model.Feedbacks_SelectedYear = year;
			prepareFeedbacks(model);
			return PartialView("~/Areas/Admin/Views/Charts/_Feedbacks.cshtml", model);
		}

		#endregion

		#region Active/Inactive Users Pie Chart

		public DashboardModel prepareActiveInactiveUsers(DashboardModel model)
		{
			model.ActiveInactiveUsers.ActiveUsers = _userService.GetAllUsers().Where(x => x.IsApproved).ToList();
			model.ActiveInactiveUsers.InActiveUsers = _userService.GetUnApprovedUsers();
			return model;
		}

		#endregion

		#region Events

		public DashboardModel prepareEvents(DashboardModel model)
		{
			var _totalWeeks = _cultureHelper.GetTotalWeeksInAMonth(model.Events_SelectedYear > 0 ? model.Events_SelectedYear : DateTime.Now.Year, model.Events_SelectedMonth > 0 ? model.Events_SelectedMonth == 1 ? model.Events_SelectedMonth : model.Events_SelectedMonth : DateTime.Now.Month);

			model.Events_SelectedMonth = model.Events_SelectedMonth > 0 ? model.Events_SelectedMonth : DateTime.Now.Month > 1 ? DateTime.Now.Month - 1 : DateTime.Now.Month;
			model.Events_SelectedYear = model.Events_SelectedYear > 0 ? model.Events_SelectedYear : DateTime.Now.Year;

			if (_totalWeeks.Count > 0)
			{
				var _events = _eventService.GetAllEvents(true).Where(x => x.IsDeleted == false).ToList();

				foreach (var weekGroup in _totalWeeks)
				{
					var _chartModel = new DashboardModel.RegisteredEvents();
					_chartModel.Events_WeekStart = Convert.ToDateTime(weekGroup.Item2).Date;
					_chartModel.Events_WeekEnd = Convert.ToDateTime(weekGroup.Item3).Date;
					var q = (from x in _events
								where x.StartDate.HasValue ? x.StartDate.Value.Date >= _chartModel.Events_WeekStart.Date : true && x.EndDate.HasValue ? x.EndDate.Value.Date <= _chartModel.Events_WeekEnd.Date : true
								select new { x.Id, x.Title }).ToList();

					_chartModel.Events = q.AsEnumerable().Select(item => new KeyValuePair<int, string>(item.Id, item.Title)).ToList();

					model.EventsChartModels.Add(_chartModel);
				}
			}

			// Load Available Months
			var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.TakeWhile(m => m != String.Empty).Select((m, i) => new { Month = i < 9 ? Convert.ToString((i + 1)) : Convert.ToString(i + 1), MonthName = m }).ToList();
			model.Events_AvailableMonths.Add(new SelectListItem { Text = "-- Select Month --", Value = "0" });
			foreach (var t in months)
				model.Events_AvailableMonths.Add(new SelectListItem { Text = t.MonthName, Value = t.Month.ToString() });

			if (model.Events_SelectedMonth > 0)
			{
				var _MonthToBeSelected = model.Events_AvailableMonths.FirstOrDefault(ss => ss.Value == model.Events_SelectedMonth.ToString().PadLeft(2, '0'));
				if (_MonthToBeSelected != null)
				{
					model.Events_AvailableMonths.ToList().ForEach(u => u.Selected = false);
					model.Events_AvailableMonths.FirstOrDefault(ss => ss.Value == model.Events_SelectedMonth.ToString().PadLeft(2, '0')).Selected = true;
				}
			}

			// Load Available Years
			var years = Enumerable.Range(DateTime.Now.Year - 2, 3).OrderByDescending(x => x).ToList();
			model.Events_AvailableYears.Add(new SelectListItem { Text = "-- Select Year --", Value = "0" });
			foreach (var t in years)
				model.Events_AvailableYears.Add(new SelectListItem { Text = t.ToString(), Value = t.ToString() });

			if (model.Events_SelectedYear > 0)
			{
				var _YearToBeSelected = model.Events_AvailableYears.FirstOrDefault(ss => ss.Value == model.Events_SelectedYear.ToString());
				if (_YearToBeSelected != null)
				{
					model.Events_AvailableYears.ToList().ForEach(u => u.Selected = false);
					model.Events_AvailableYears.FirstOrDefault(ss => ss.Value == model.Events_SelectedYear.ToString()).Selected = true;
				}
			}

			return model;
		}

		public ActionResult DrawEventsChart(int month, int year)
		{
			if (month == 0 && year == 0)
				throw new Exception("Invalid Month And Year Selected!");

			ModelState.Clear();
			var model = new DashboardModel();
			model.Events_SelectedMonth = month;
			model.Events_SelectedYear = year;
			prepareEvents(model);
			return PartialView("~/Areas/Admin/Views/Charts/_Events.cshtml", model);
		}

		#endregion

		public ActionResult Logo()
		{
			var model = new AdminHeaderModel();
			var logo = _pictureService.GetHomeLogo();
			if (logo != null)
			{
				model.LogoEnabled = true;
				model.pictures = new EF.Core.Data.Picture
				{
					AlternateText = logo.AlternateText,
					CreatedOn = logo.CreatedOn,
					Height = logo.Height,
					Id = logo.Id,
					IsActive = logo.IsActive,
					IsLogo = logo.IsLogo,
					IsThumb = logo.IsThumb,
					Size = logo.Size,
					Url = logo.Url,
					UserId = logo.UserId,
					Width = logo.Width
				};
			}

			return PartialView("_logo", model);
		}

		public ActionResult LogOff()
		{
			//standard logout 
			_authenticationService.SignOut();
			return RedirectToRoute("Root");
		}

		//[ChildActionOnly]
		public ActionResult TopLinks()
		{
			var user = _userContext.CurrentUser;

			if (user == null)
			{
				var model = new TopLinksModel
				{
					IsAuthenticated = false
				};

				return PartialView(model);
			}
			else
			{
				var model = new TopLinksModel
				{
					IsAuthenticated = true,
					user = user,
					RoleId = user.Roles.FirstOrDefault().Id,
					RoleName = user.Roles.FirstOrDefault().RoleName
				};

				return PartialView(model);
			}
		}

		public ActionResult UserApproveList()
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			var model = new NotificationModel();
			model.Users = _userService.GetAllUsers(true, false).Select(x => new UserModel()
			{
				UserId = x.Id,
				Username = x.UserName,
				CreatedOn = x.CreatedOn
			}).ToList();

			// Load Available Roles
			model.AvailableRoles.Add(new SelectListItem { Text = "-- Select Template --", Value = "0" });
			foreach (var t in _roleService.GetAllRoles())
				model.AvailableRoles.Add(new SelectListItem { Text = t.RoleName, Value = t.Id.ToString() });

			return View(model);
		}


		[HttpPost]
		public ActionResult ApproveSelected(ICollection<int> selectedIds)
		{
			if (!_permissionService.Authorize("ManageDashboard"))
				return AccessDeniedView();

			if (selectedIds != null)
			{
				_userService.ApproveUsers(_userService.GetUsersByIds(selectedIds.ToArray()).ToList());
			}

			return Json(new { Result = true });
		}

		[HttpPost]
		public ActionResult RejectSelected(ICollection<int> selectedIds)
		{
			if (!_permissionService.Authorize("ManageDashboard"))
				return AccessDeniedView();

			if (selectedIds != null)
			{
				_userService.RejectUsers(_userService.GetUsersByIds(selectedIds.ToArray()).ToList());
			}

			return Json(new { Result = true });
		}


		[HttpPost]
		public ActionResult ApproveReject(string approvereject = null, int roleid = 0, int userid = 0)
		{
			if (!_permissionService.Authorize("ManageDashboard"))
				return AccessDeniedView();

			var _user = _userService.GetUserById(userid);
			var _role = _roleService.GetRoleById(roleid);
			switch (approvereject)
			{
				case "approve":
					if (_user != null)
					{
						_user.IsApproved = true;
						_user.Roles.Add(_role);
						_userService.Update(_user);
					}
					break;
				case "reject":
					if (_user != null)
					{
						_user.IsApproved = false;
						_user.IsDeleted = true;
						_userService.Update(_user);
					}
					break;
				default:
					break;

			}

			ViewBag.Result = "User Approved Successfully.";
			return RedirectToAction("UserApproveList", "Dashboard");
		}

		[ChildActionOnly]
		public ActionResult SideBar(string activesettings = null)
		{
			if (!_permissionService.Authorize("ManageDashboard"))
				return AccessDeniedView();

			var _user = _userContext.CurrentUser;
			var model = new LeftSideBarModel();
			model.ActiveSettings = "";

			if (!String.IsNullOrEmpty(activesettings))
				model.ActiveSettings = activesettings;

			if (_user != null)
			{
				model.adminUserModel.UserId = _user.UserId;
				model.adminUserModel.UserName = _user.UserName;
			}

			return View("~/Areas/Admin/Views/Shared/_Links.cshtml", model);
		}

		[ChildActionOnly]
		public ActionResult BellNotifications()
		{
			var notification = new NotificationModel();
			notification.Users = _userService.GetUnApprovedUsers().Select(x => new UserModel()
			{
				Id = x.Id,
				Username = x.UserName,
				CreatedOn = x.CreatedOn
			}).ToList();

			notification.SystemLogs = _systemLogService.GetAllSystemLogs(fromUtc: DateTime.UtcNow, toUtc: DateTime.UtcNow).Select(s => new SystemLogModel()
			{
				Date = s.CreatedOn.Date,
				EntityId = s.EntityId,
				Id = s.Id,
				EntityTypeName = s.EntityTypeName,
				IpAddress = s.IpAddress,
				Message = s.Message
			}).OrderByDescending(s => s.Date).Take(5).ToList();

			notification.AuditRequests = _auditService.GetAllAuditsByDate(DateTime.UtcNow).Take(3).Select(a => new AuditModel()
			{
				AuditLogId = a.AuditLogId,
				EventDateUTC = a.EventDateUTC,
				EventType = a.EventType,
				LogDetails = a.LogDetails.Select(l => new TrackerEnabledDbContext.Common.Models.AuditLogDetail()
				{
					AuditLogId = l.AuditLogId,
					Id = l.Id,
					Log = l.Log,
					NewValue = l.NewValue,
					OriginalValue = l.OriginalValue,
					PropertyName = l.PropertyName
				}).ToList(),
				UserName = a.UserName
			}).ToList();

			notification.NotificationCount = notification.Users.Count + notification.SystemLogs.Count + notification.AuditRequests.Count;

			return View("~/Areas/Admin/Views/Common/BellNotifications.cshtml", notification);
		}

		[ChildActionOnly]
		public PartialViewResult AcadmicYear()
		{
			var model = new AcadmicYearModel();
			model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears(null).Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Name,
				Selected = x.IsActive ? true : false
			}).ToList();

			var activeYear = model.AvailableAcadmicYears.FirstOrDefault(x => x.Selected);

			if (activeYear != null)
			{
				model.Name = activeYear.Value;
				_userContext.CurrentAcadmicYear = _smsService.GetAcadmicYearByName(model.Name);
			}

			return PartialView(model);
		}

	}
}
