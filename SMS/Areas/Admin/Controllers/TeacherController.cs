using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SMS.Mappers;
using EF.Core;
using EF.Core.Data;
using EF.Services;
using EF.Services.Service;
using EF.Services.Http;
using SMS.Models;
using System.Text;

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
            this._userService = userService;
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._sliderService = sliderService;
            this._settingService = settingService;
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._auditService = auditService;
            this._smsService = smsService;
            this._fileService = fileService;
            this._urlService = urlService;
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

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(TeacherModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageTeachers"))
                return AccessDeniedView();

            // Check for duplicate teacher, if any
            var _allActiveTeachers = _smsService.CheckTeacherExists(model.Name, model.Id);
            if (_allActiveTeachers)
                ModelState.AddModelError("Name", "A Teacher with the same name already exists. Please choose a different name.");

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

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(TeacherModel model)
        {
            if (!_permissionService.Authorize("ManageTeachers"))
                return AccessDeniedView();

            if (_smsService.CheckTeacherExists(model.Name))
                ModelState.AddModelError("Name", "A Teacher with same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var teacher = model.ToEntity();
                _smsService.InsertTeacher(teacher);

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

        #endregion

    }
}
