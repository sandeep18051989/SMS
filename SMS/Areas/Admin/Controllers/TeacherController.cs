using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class TeacherController : AdminAreaController
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
		private readonly IFileService _fileService;
		private readonly IUrlService _urlService;

		#endregion Fileds

		#region Constructor

		public TeacherController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, IAuditService auditService, ISMSService smsService, IFileService fileService, IUrlService urlService)
		{
			_userService = userService;
			_pictureService = pictureService;
			_userContext = userContext;
			_sliderService = sliderService;
			_settingService = settingService;
			_roleService = roleService;
			_permissionService = permissionService;
			_auditService = auditService;
			_smsService = smsService;
			_fileService = fileService;
			_urlService = urlService;
		}

		#endregion

		#region Utilities

		public ActionResult LoadGrid()
		{
			try
			{
				var draw = Request.Form.GetValues("draw").FirstOrDefault();
				var start = Request.Form.GetValues("start").FirstOrDefault();
				var length = Request.Form.GetValues("length").FirstOrDefault();
				var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() + "][name]")?.FirstOrDefault();
				var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();
				var searchValue = Request.Form.GetValues("search[value]")?.FirstOrDefault();


				//Paging Size (10,20,50,100)    
				int pageSize = length != null ? Convert.ToInt32(length) : 0;
				int skip = start != null ? Convert.ToInt32(start) : 0;
				int recordsTotal = 0;

				// Getting all data    
				var teacherData = (from tempteachers in _smsService.GetAllTeachers() select tempteachers);

				//Search    
				if (!string.IsNullOrEmpty(searchValue))
				{
					teacherData = teacherData.Where(m => m.Name.Contains(searchValue) || m.Description.Contains(searchValue));
				}

				//total number of rows count     
				recordsTotal = teacherData.Count();
				//Paging     
				var data = teacherData.Skip(skip).Take(pageSize).ToList();

				//Returning Json Data 
				return new JsonResult()
				{
					Data = new
					{
						draw = draw,
						recordsFiltered = recordsTotal,
						recordsTotal = recordsTotal,
						data = data.Select(x => new TeacherModel()
						{
							Id = x.Id,
							Name = x.Name.Trim(),
							Username = x.EmployeeId > 0 ? _smsService.GetEmployeeById(x.EmployeeId).Username : "",
							PictureSrc = x.ProfilePictureId > 0 ? _pictureService.GetPictureById(x.ProfilePictureId)?.PictureSrc : "",
							AcadmicYear = x.AcadmicYearId > 0 ? _smsService.GetAcadmicYearById(x.AcadmicYearId).Name : "",
							Url = Url.RouteUrl("Teacher", new { name = x.GetSystemName() }, "http")
						})
					},
					ContentEncoding = Encoding.Default,
					ContentType = "application/json",
					JsonRequestBehavior = JsonRequestBehavior.AllowGet,
					MaxJsonLength = int.MaxValue
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		[HttpPost]
		public ActionResult LoadFileGrid(int id)
		{
			try
			{
				var fileData = (from associatedfile in _fileService.GetAllFilesByTeacher(id) select associatedfile).OrderByDescending(eve => eve.CreatedOn).ToList();
				return new JsonResult()
				{
					Data = new
					{
						data = fileData.Select(x => new FileListModel()
						{
							Id = x.Id,
							Title = !string.IsNullOrEmpty(x.Title) ? x.Title.Trim() : "",
							Type = !string.IsNullOrEmpty(x.Type) ? x.Type.Trim() : "",
							TeacherId = id,
							FileSrc = !string.IsNullOrEmpty(x.Src) ? x.Src.Trim() : "",
						})
					},
					ContentEncoding = Encoding.Default,
					ContentType = "application/json",
					JsonRequestBehavior = JsonRequestBehavior.AllowGet,
					MaxJsonLength = int.MaxValue
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		[HttpPost]
		public ActionResult LoadSubjectGrid(int id)
		{
			try
			{
				var subjectData = (from associatedsubject in _smsService.GetAllSubjectsByTeacher(id) select associatedsubject).OrderByDescending(eve => eve.CreatedOn).ToList();
				return new JsonResult()
				{
					Data = new
					{
						data = subjectData.Select(x => new SubjectModel()
						{
							Id = x.Id,
							Name = x.Name.Trim(),
							Code = x.Code.Trim(),
							SubjectUniqueId = x.SubjectUniqueId
						})
					},
					ContentEncoding = Encoding.Default,
					ContentType = "application/json",
					JsonRequestBehavior = JsonRequestBehavior.AllowGet,
					MaxJsonLength = int.MaxValue
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		[HttpPost]
		public ActionResult LoadClassDivisionGrid(int id)
		{
			try
			{
				var classdivisionData = (from associatedclassdivision in _smsService.GetAllClassDivisionsByTeacher(id) select associatedclassdivision).OrderByDescending(eve => eve.CreatedOn).ToList();
				return new JsonResult()
				{
					Data = new
					{
						data = classdivisionData.Select(x => new ClassRoomDivisionModel()
						{
							Id = x.Id,
							Class = x.Class.Name,
							Division = x.Division.Name,
							ClassRoom = x.ClassRoom.Number
						})
					},
					ContentEncoding = Encoding.Default,
					ContentType = "application/json",
					JsonRequestBehavior = JsonRequestBehavior.AllowGet,
					MaxJsonLength = int.MaxValue
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		#endregion

		#region Action Methods

		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			var model = new TeacherModel();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Teacher Id Missing");

			var teacher = _smsService.GetTeacherById(id);
			var model = teacher.ToModel();

			model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem() { Text = x.Username, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
			model.AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
			model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
			{
				Text = x.Name.Trim(),
				Value = x.Id.ToString(),
				Selected = x.IsActive
			}).ToList();
			model.AvailablePersonalityStatuses = (from PersonalityStatus d in Enum.GetValues(typeof(PersonalityStatus))
															  select new SelectListItem
															  {
																  Text = EnumExtensions.GetDescriptionByValue<PersonalityStatus>(Convert.ToInt32(d)),
                                                                  Value = Convert.ToInt32(d).ToString(),
																  Selected = (Convert.ToInt32(d) == model.PersonalityStatusId)
															  }).ToList();

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(TeacherModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			var teacher = _smsService.GetTeacherById(model.Id);
			if (ModelState.IsValid)
			{
				teacher = model.ToEntity(teacher);
				teacher.ModifiedOn = DateTime.Now;
				_smsService.UpdateTeacher(teacher);

				// Save URL Record
				model.SystemName = teacher.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(teacher, model.SystemName);

			}
			else
			{
				model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem() { Text = x.Username, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
				model.AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
				model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
				{
					Text = x.Name.Trim(),
					Value = x.Id.ToString(),
					Selected = x.IsActive
				}).ToList();
				model.AvailablePersonalityStatuses = (from PersonalityStatus d in Enum.GetValues(typeof(PersonalityStatus))
																  select new SelectListItem
																  {
																	  Text = EnumExtensions.GetDescriptionByValue<PersonalityStatus>(Convert.ToInt32(d)),
                                                                      Value = Convert.ToInt32(d).ToString(),
																	  Selected = (Convert.ToInt32(d) == model.PersonalityStatusId)
																  }).ToList();
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
			model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
			{
				Text = x.Name.Trim(),
				Value = x.Id.ToString(),
				Selected = x.IsActive
			}).ToList();
			model.AvailablePersonalityStatuses = (from PersonalityStatus d in Enum.GetValues(typeof(PersonalityStatus))
															  select new SelectListItem
															  {
																  Text = EnumExtensions.GetDescriptionByValue<PersonalityStatus>(Convert.ToInt32(d)),
                                                                  Value = Convert.ToInt32(d).ToString(),
																  Selected = (Convert.ToInt32(d) == model.PersonalityStatusId)
															  }).ToList();
			return View(model);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(TeacherModel model)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (ModelState.IsValid)
			{
				var teacher = model.ToEntity();
                teacher.CreatedOn = teacher.ModifiedOn = DateTime.Now;
                teacher.UserId = _userContext.CurrentUser.Id;
				_smsService.InsertTeacher(teacher);

				var employ = _smsService.GetEmployeeById(teacher.EmployeeId);

				// Add A User
				var newUser = new User();
				newUser.Email = employ.Email;
				newUser.CreatedOn = newUser.ModifiedOn = DateTime.Now;
				newUser.IsActive = false;
				newUser.IsApproved = false;
				newUser.IsDeleted = false;
				newUser.Password = CodeHelper.GenerateRandomDigitCode(6);
				newUser.SeoName = employ.Email;
				newUser.UserGuid = Guid.NewGuid();
				newUser.UserId = _userContext.CurrentUser.Id;
				newUser.UserName = teacher.Username;
				newUser.Roles.Add(_roleService.GetRoleByName("Teacher"));
				_userService.Insert(newUser);

				teacher.ImpersonateId = newUser.Id;
				_smsService.UpdateTeacher(teacher);

				// Save URL Record
				model.SystemName = teacher.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(teacher, model.SystemName);

			}
			else
			{
				model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem() { Text = x.Username, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
				model.AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
				model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
				{
					Text = x.Name.Trim(),
					Value = x.Id.ToString(),
					Selected = x.IsActive
				}).ToList();
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

		#region Subject Association

		public JsonResult GetAllSubjectsByTeacher(int teacherid)
		{
			var allSubjects = _smsService.GetAllSubjects();
			var subjectsByTeacher = _smsService.GetAllSubjectsByTeacher(id: teacherid);

			//Returning Json Data 
			return new JsonResult()
			{
				Data = allSubjects.Select(x => new SubjectModel()
				{
					IsActive = x.IsActive,
					UserId = x.UserId,
					Name = x.Name.Trim(),
					AcadmicYearId = x.AcadmicYearId,
					Code = x.Code,
					Remarks = x.Remarks,
					Id = x.Id,
					Selected = subjectsByTeacher.Any(y => y.Id == x.Id)
				}).OrderBy(x => x.Code).ToList(),
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}
		[HttpPost]
		public ActionResult UpdateSubjectsForTeacher(int id, int[] subjects)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (id == 0)
				throw new ArgumentNullException("id");

			var user = _userContext.CurrentUser;
			var objTeacher = _smsService.GetTeacherById(id);
			if (objTeacher != null)
			{
				var objSubjects = _smsService.GetAllSubjectsByTeacher(id);
				if (subjects != null && subjects.Length > 0)
				{
					foreach (var subjectid in subjects)
					{
						var checkRecords = objTeacher.Subjects.Any(x => x.Id == subjectid);
						if (!checkRecords)
						{
							var newSubjectToAdd = _smsService.GetSubjectById(subjectid);
							if (newSubjectToAdd != null)
							{
								objTeacher.Subjects.Add(newSubjectToAdd);
								_smsService.UpdateTeacher(objTeacher);
							}
						}
					}
				}
				else
				{
					objTeacher.Subjects.Clear();
					_smsService.UpdateTeacher(objTeacher);
				}
			}
			ViewBag.Result = "Teacher updated Successfully";
			return Json(new { Result = true });
		}

		[HttpPost]
		public ActionResult RemoveSubjectFromTeacher(int id, int subjectid)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Teacher id not found");

			var objTeacher = _smsService.GetTeacherById(id);
			if (objTeacher != null)
			{
				var selectSubject = objTeacher.Subjects.FirstOrDefault(x => x.Id == subjectid);
				if (selectSubject != null)
				{
					objTeacher.Subjects.Remove(selectSubject);
					_smsService.UpdateTeacher(objTeacher);
				}
			}

			SuccessNotification("Subject removed successfully from selected teacher.");
			return new JsonResult()
			{
				Data = true,
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}

		#endregion

		#region Division Association

		public JsonResult GetAllDivisionsByTeacher(int teacherid)
		{
			var allDivisions = _smsService.GetAllClassRoomDivisions();
			var divisionsByTeacher = _smsService.GetAllClassDivisionsByTeacher(id: teacherid);

			//Returning Json Data 
			return new JsonResult()
			{
				Data = allDivisions.Select(x => new ClassRoomDivisionModel()
				{
					UserId = x.UserId,
					Id = x.Id,
					Class = x.ClassId.HasValue ? _smsService.GetClassById(x.ClassId.Value).Name : "",
					Division = x.DivisionId.HasValue ? _smsService.GetDivisionById(x.DivisionId.Value).Name : "",
					ClassRoom = x.ClassRoomId.HasValue ? _smsService.GetClassRoomById(x.ClassRoomId.Value).Number : "",
					ClassId = x.ClassId.Value,
					DivisionId = x.DivisionId.Value,
					ClassRoomId = x.ClassRoomId.Value,
					Selected = divisionsByTeacher.Any(y => y.Id == x.Id)
				}).OrderBy(x => x.Class).ToList(),
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}

		[HttpPost]
		public ActionResult UpdateDivisionsForTeacher(int id, int[] divisions)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (id == 0)
				throw new ArgumentNullException("id");

			var user = _userContext.CurrentUser;
			var objTeacher = _smsService.GetTeacherById(id);
			if (objTeacher != null)
			{
				var objDivisions = _smsService.GetAllClassDivisionsByTeacher(id);
				if (divisions != null && divisions.Length > 0)
				{
					foreach (var divisionid in divisions)
					{
						var checkRecords = objTeacher.ClassRoomDivisions.Any(x => x.Id == divisionid);
						if (!checkRecords)
						{
							var newDivisionToAdd = _smsService.GetClassRoomDivisionById(divisionid);
							if (newDivisionToAdd != null)
							{
								objTeacher.ClassRoomDivisions.Add(newDivisionToAdd);
								_smsService.UpdateTeacher(objTeacher);
							}
						}
					}
				}
				else
				{
					objTeacher.ClassRoomDivisions.Clear();
					_smsService.UpdateTeacher(objTeacher);
				}
			}
			ViewBag.Result = "Teacher updated Successfully";
			return Json(new { Result = true });
		}

		[HttpPost]
		public ActionResult RemoveDivisionFromTeacher(int id, int divisionid)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Teacher id not found");

			var objTeacher = _smsService.GetTeacherById(id);
			if (objTeacher != null)
			{
				var selectDivision = objTeacher.ClassRoomDivisions.FirstOrDefault(x => x.Id == divisionid);
				if (selectDivision != null)
				{
					objTeacher.ClassRoomDivisions.Remove(selectDivision);
					_smsService.UpdateTeacher(objTeacher);
				}
			}

			SuccessNotification("Division removed successfully from selected teacher.");
			return new JsonResult()
			{
				Data = true,
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}

		#endregion

		#region Teacher Files

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult TeacherFileAddUpdate(int teacherId, int fileId, string title, string type)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (fileId == 0)
				throw new ArgumentException();

			var thisteacher = _smsService.GetTeacherById(teacherId);
			if (thisteacher == null)
				throw new ArgumentException("No teacher found with the specified id");

			var file = _fileService.GetFileById(fileId);
			if (file == null)
				throw new ArgumentException("No file found with the specified id");

			if (!string.IsNullOrEmpty(title) && !thisteacher.Files.Any(x => x.Title.Trim().ToLower() == title.Trim().ToLower()))
			{
				file.Title = title;
				file.Type = type;
				thisteacher.Files.Add(file);
				_smsService.UpdateTeacher(thisteacher);
			}
			else if (fileId > 0 && !thisteacher.Files.Any(x => x.Id == fileId))
			{
				file.Title = title;
				file.Type = type;
				thisteacher.Files.Add(file);
				_smsService.UpdateTeacher(thisteacher);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult DeleteTeacherFile(int id)
		{
			if (!_permissionService.Authorize("ManageTeachers"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("File id not found");

			var fileRecord = _fileService.GetFileById(id);
			if (fileRecord != null)
			{
				_fileService.Delete(fileRecord.Id);
			}

			SuccessNotification("Teacher file deleted successfully");
			return new JsonResult()
			{
				Data = true,
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}

		#endregion

		#endregion

	}
}
