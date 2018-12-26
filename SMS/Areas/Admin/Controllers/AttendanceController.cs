using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class AttendanceController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		public readonly ISettingService _settingService;
		public readonly IRoleService _roleService;
		private readonly IPermissionService _permissionService;
		private readonly IAuditService _auditService;
		private readonly ISMSService _smsService;

		#endregion Fileds

		#region Constructor

		public AttendanceController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, IAuditService auditService, ISMSService smsService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._roleService = roleService;
			this._permissionService = permissionService;
			this._auditService = auditService;
			this._smsService = smsService;
		}

		#endregion

		#region Action Methods

		public ActionResult EmployeeAttendanceList(int? page)
		{
			if (!_permissionService.Authorize("ManageEmployeeAttendance"))
				return AccessDeniedView();

			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			PagerParams param = new PagerParams(10, currentPageIndex, -1);
			var employeeAttendance = _smsService.GetEmployeeAttendanceByDateAndEmployee(param, null, 0);
			var model = new EmployeeAttendanceOverviewModel();

			if (employeeAttendance.TotalCount > 0)
				model.LoadPagedList(employeeAttendance);

			return View(model);
		}

		public ActionResult StudentAttendanceList()
		{
			if (!_permissionService.Authorize("ManageStudentAttendance"))
				return AccessDeniedView();

			var model = new List<TeacherModel>();
			model = _smsService.GetAllTeachers(null).Where(x => !x.IsDeleted).Select(x => new TeacherModel()
			{
				Id = x.Id,
				IsActive = x.IsActive,
				IsDeleted = x.IsDeleted,
				EmployeeId = x.EmployeeId,
				QualificationId = x.QualificationId,
				UserId = x.UserId
			}).ToList();

			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Teacher Id Missing");

			var teacher = _smsService.GetTeacherById(id);
			var model = new TeacherModel()
			{
				Id = teacher.Id,
				IsActive = teacher.IsActive,
				IsDeleted = teacher.IsDeleted,
				AcadmicYear = teacher.AcadmicYearId > 0 ? _smsService.GetAcadmicYearById(teacher.AcadmicYearId) : null,
				EmployeeId = teacher.EmployeeId,
				QualificationId = teacher.QualificationId,
				UserId = teacher.UserId,
				AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem() { Text = x.Username, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList(),
				AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList(),
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(TeacherModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			// Get Active Acadmic Year
			var acadmicyear = _smsService.GetActiveAcadmicYear();

			// Check for duplicate teacher, if any
			var _allActiveTeachers = _smsService.SearchTeachers(true, 0, 0, 0, acadmicyear.Id);

			if (_allActiveTeachers.Any(u => u.Name.Trim().ToLower() == model.Name.Trim().ToLower() && u.Id != model.Id))
				ModelState.AddModelError("Name", "A Teacher with the same name already exists. Please choose a different name.");

			var _teacher = _smsService.GetTeacherById(model.Id);

			if (ModelState.IsValid)
			{
				_teacher.Id = model.Id;
				_teacher.IsActive = model.IsActive;
				_teacher.IsDeleted = model.IsDeleted;
				_teacher.UserId = _userContext.CurrentUser.Id;
				_teacher.EmployeeId = model.EmployeeId;
				_teacher.QualificationId = model.QualificationId;
				_smsService.UpdateTeacher(_teacher);
			}
			else
			{
				return View(model);
			}

			SuccessNotification("Teacher updated successfully.");
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			var model = new TeacherModel();
			model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem() { Text = x.Username, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
			model.AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();

			return View(model);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(TeacherModel model)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			// Check for duplicate teacher, if any
			var _teacher = _smsService.GetTeachersByName(model.Name, null);

			if (_teacher != null)
				ModelState.AddModelError("Name", "A Teacher with same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var newTeacher = new Teacher()
				{
					CreatedOn = DateTime.Now,
					IsActive = model.IsActive,
					ModifiedOn = DateTime.Now,
					IsDeleted = false,
					Name = model.Name,
					EmployeeId = model.EmployeeId,
					QualificationId = model.QualificationId,
					UserId = _userContext.CurrentUser.Id
				};
				_smsService.InsertTeacher(newTeacher);
			}
			else
			{
				return View(model);
			}

			SuccessNotification("User created successfully.");
			return RedirectToAction("List");
		}

		[HttpPost]
		public ActionResult DeleteSelected(ICollection<int> selectedIds)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (selectedIds != null)
			{
				_smsService.DeleteTeachers(_smsService.GetTeachersByIds(selectedIds.ToArray()).ToList());
			}

			return Json(new { Result = true });
		}

		[HttpPost]
		public ActionResult ToggleTeacher(string id)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (String.IsNullOrEmpty(id))
				throw new Exception("Id Not Found");

			var _teacher = _smsService.GetTeacherById(Convert.ToInt32(id));

			if (_teacher != null)
				_smsService.ToggleTeacher(Convert.ToInt32(id));

			if (_teacher.IsActive)
			{
				SuccessNotification("Teacher activated successfully.");
			}
			else
			{
				SuccessNotification("Teacher de-activated successfully.");
			}
			return View("List");
		}


		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			if (id != 1)
				_userService.Delete(id);

			SuccessNotification("Teacher deleted successfully.");
			return RedirectToAction("List");
		}

		#endregion

	}
}
