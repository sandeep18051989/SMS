using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;
using EF.Services;
using EF.Core.Enums;

namespace SMS.Areas.Admin.Controllers
{
    public class ExamController : AdminAreaController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IUserContext _userContext;
        private readonly ISettingService _settingService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly ISMSService _smsService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;

        #endregion Fileds

        #region Constructor

        public ExamController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
        {
            this._userService = userService;
            this._userContext = userContext;
            this._settingService = settingService;
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._smsService = smsService;
            this._commentService = commentService;
            this._replyService = replyService;
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
                var examData = (from tempexams in _smsService.GetAllExams() select tempexams);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    examData = examData.Where(m => m.ExamName.Contains(searchValue));
                }

                //total number of rows count     
                var lstExams = examData as Exam[] ?? examData.ToArray();
                recordsTotal = lstExams.Count();
                //Paging     
                var data = lstExams.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new ExamModel()
                        {
                            ExamName = x.ExamName,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            PassingMarks = x.PassingMarks,
                            MaxMarks = x.MaxMarks,
                            IsActive = x.IsActive,
                            Id = x.Id,
                            StringStartDate = x.StartDate != null ? x.StartDate.Value.ToString("dd MMMM yyyy") : "",
                            StringEndDate = x.EndDate != null ? x.EndDate.Value.ToString("dd MMMM yyyy") : ""
                        }).OrderBy(x => x.ExamName).ToList()
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
        public ActionResult LoadDivisionExamGrid(int id)
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
                var examData = (from tempexams in _smsService.GetAllExamsByClassDivision(id) select tempexams);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    examData = examData.Where(m => m.Exam.ExamName.Contains(searchValue));
                }

                //total number of rows count     
                var lstExams = examData as DivisionExam[] ?? examData.ToArray();
                recordsTotal = lstExams.Count();
                //Paging     
                var data = lstExams.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new ExamModel()
                        {
                            ExamName = x.Exam.ExamName,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            PassingMarks = x.PassingMarks,
                            MaxMarks = x.MaxMarks,
                            Id = x.Id,
                            StringStartDate = x.StartDate != null ? x.StartDate.Value.ToString("dd MMMM yyyy") : "",
                            StringEndDate = x.EndDate != null ? x.EndDate.Value.ToString("dd MMMM yyyy") : ""
                        }).OrderBy(x => x.ExamName).ToList()
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
        public ActionResult LoadTeacherExamGrid(int id)
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
                var examData = (from tempexams in _smsService.GetAllExamsByTeacher(id) select tempexams);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    examData = examData.Where(m => m.Exam.ExamName.Contains(searchValue));
                }

                //total number of rows count     
                var lstExams = examData as TeacherExam[] ?? examData.ToArray();
                recordsTotal = lstExams.Count();
                //Paging     
                var data = lstExams.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new ExamModel()
                        {
                            ExamName = x.Exam.ExamName,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            PassingMarks = x.PassingMarks,
                            MaxMarks = x.MaxMarks,
                            Id = x.Id,
                            StringStartDate = x.StartDate != null ? x.StartDate.Value.ToString("dd MMMM yyyy") : "",
                            StringEndDate = x.EndDate != null ? x.EndDate.Value.ToString("dd MMMM yyyy") : ""
                        }).OrderBy(x => x.ExamName).ToList()
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
        public ActionResult LoadStudentExamGrid(int id)
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
                var examData = (from tempexams in _smsService.GetAllExamsByStudent(id) select tempexams);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    examData = examData.Where(m => m.Exam.ExamName.Contains(searchValue));
                }

                //total number of rows count     
                var lstExams = examData as StudentExam[] ?? examData.ToArray();
                recordsTotal = lstExams.Count();
                //Paging     
                var data = lstExams.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new ExamModel()
                        {
                            ExamName = x.Exam.ExamName,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            PassingMarks = x.PassingMarks,
                            MaxMarks = x.MaxMarks,
                            Id = x.Id,
                            StringStartDate = x.StartDate != null ? x.StartDate.Value.ToString("dd MMMM yyyy") : "",
                            StringEndDate = x.EndDate != null ? x.EndDate.Value.ToString("dd MMMM yyyy") : ""
                        }).OrderBy(x => x.ExamName).ToList()
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

        public JsonResult GetAllExams(int classdivisionid)
        {
            var allExams = _smsService.GetAllExams();
            var examByClassDivision = _smsService.GetAllExamsByClassDivision(classdivisionid);

            //Returning Json Data 
            return new JsonResult()
            {
                Data = allExams.Select(x => new ExamModel()
                {
                    IsActive = x.IsActive,
                    UserId = x.UserId,
                    ExamName = x.ExamName.Trim(),
                    AcadmicYearId = x.AcadmicYearId,
                    AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    MaxMarks = x.MaxMarks,
                    PassingMarks = x.PassingMarks,
                    Id = x.Id,
                    Selected = examByClassDivision.Any(y => y.ExamId == x.Id),
                    AvailableClassRooms = _smsService.GetVacantClassRoomsForExams().Select(y => new SelectListItem()
                    {
                        Text = y.Number,
                        Value = y.Id.ToString(),
                    }).OrderBy(y => y.Text).ToList()
                }).OrderBy(x => x.ExamName).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }


        public JsonResult CheckExamNameExists(string name, int? id)
        {
            return new JsonResult()
            {
                Data = _smsService.CheckExamExists(name, id),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion
        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            var model = new ExamModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new ExamModel();
            var objExam = _smsService.GetExamById(id);
            if (objExam != null)
            {
                model = objExam.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ExamModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            // Check for duplicate exam, if any
            var checkExam = _smsService.CheckExamExists(model.ExamName, model.Id);
            if (checkExam)
                ModelState.AddModelError("ExamName", "An Exam with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objExam = _smsService.GetExamById(model.Id);
                if (objExam != null)
                {
                    objExam = model.ToEntity(objExam);
                    objExam.ModifiedOn = DateTime.Now;
                    objExam.UserId = _userContext.CurrentUser.Id;
                    _smsService.UpdateExam(objExam);
                }
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.IsActive
                }).ToList();
                return View(model);
            }

            SuccessNotification("Exam updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            var model = new ExamModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();

            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ExamModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            // Check for duplicate exam, if any
            var checkExam = _smsService.CheckExamExists(model.ExamName, model.Id);
            if (checkExam)
                ModelState.AddModelError("ExamName", "An Exam with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objExam = model.ToEntity();
                objExam.CreatedOn = objExam.ModifiedOn = DateTime.Now;
                objExam.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertExam(objExam);

                SuccessNotification("Exam created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objExam.Id });
                }
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.IsActive
                }).ToList();
                return View(model);
            }

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _roleService.Delete(id);

            SuccessNotification("Exam deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusExam(id);
            ViewBag.Result = "Exam updated Successfully";

            return Json(new { Result = true });
        }

        #region Division Exam

        public ActionResult DivisionExams(int id)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var classDivision = _smsService.GetClassRoomDivisionById(id);
            if (classDivision == null)
                throw new ArgumentNullException("classroomdivision");

            var model = new DivisionExamModel();
            model.ClassId = classDivision.ClassId.Value;
            model.DivisionId = classDivision.DivisionId.Value;
            model.ClassRoomId = classDivision.ClassRoomId.Value;
            model.Class = classDivision.Class.Name;
            model.Division = classDivision.Division.Name;
            model.ClassRoom = classDivision.ClassRoom.Number;

            var examsAlreadyAssociated = _smsService.GetAllExamsByClassDivision(id);
            model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
            {
                Text = x.ExamName.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ExamId
            }).Where(x => !examsAlreadyAssociated.Any(y => y.ExamId.ToString() == x.Value)).ToList();

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.AcadmicYearId
            }).ToList();

            var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
            model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
            {
                Text = x.Number.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ClassRoomId
            }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

            model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                          select new SelectListItem
                                          {
                                              Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                              Value = Convert.ToInt32(d).ToString(),
                                              Selected = false
                                          }).ToList();

            model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                             select new SelectListItem
                                             {
                                                 Text = d.ToString(),
                                                 Value = Convert.ToInt32(d).ToString(),
                                                 Selected = false
                                             }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult DivisionExams(DivisionExamModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var objDivisionExam = model.ToEntity();
                objDivisionExam.Id = 0;
                objDivisionExam.CreatedOn = objDivisionExam.ModifiedOn = DateTime.Now;
                objDivisionExam.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertDivisionExam(objDivisionExam);
            }
            else
            {
                var examsAlreadyAssociated = _smsService.GetAllExamsByClassDivision(model.DivisionId);
                model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
                {
                    Text = x.ExamName.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ExamId
                }).Where(x => !examsAlreadyAssociated.Any(y => y.ExamId.ToString() == x.Value)).ToList();

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.AcadmicYearId
                }).ToList();

                var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
                model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
                {
                    Text = x.Number.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ClassRoomId
                }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

                model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                              select new SelectListItem
                                              {
                                                  Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                                  Value = Convert.ToInt32(d).ToString(),
                                                  Selected = model.GradeSystemId == Convert.ToInt32(d)
                                              }).ToList();

                model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                                 select new SelectListItem
                                                 {
                                                     Text = d.ToString(),
                                                     Value = Convert.ToInt32(d).ToString(),
                                                     Selected = model.ResultStatusId == Convert.ToInt32(d)
                                                 }).ToList();
                return View(model);
            }

            SuccessNotification("Exam updated successfully.");
            return RedirectToAction("DivisionExams", new { id = model.Id });
        }


        public ActionResult EditDivisionExam(int id)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var examDivision = _smsService.GetDivisionExamMappingById(id);
            if (examDivision == null)
                throw new ArgumentNullException("Exam Division");

            var model = examDivision.ToModel();
            var examsAlreadyAssociated = _smsService.GetAllExamsByClassDivision(id);

            model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
            {
                Text = x.ExamName.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ExamId
            }).Where(x => !examsAlreadyAssociated.Any(y => (y.ExamId.ToString() == x.Value)) || model.ExamId.ToString() == x.Value).ToList();

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.AcadmicYearId
            }).ToList();

            var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
            model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
            {
                Text = x.Number.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ClassRoomId
            }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

            model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                          select new SelectListItem
                                          {
                                              Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                              Value = Convert.ToInt32(d).ToString(),
                                              Selected = false
                                          }).ToList();

            model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                             select new SelectListItem
                                             {
                                                 Text = d.ToString(),
                                                 Value = Convert.ToInt32(d).ToString(),
                                                 Selected = false
                                             }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditDivisionExam(DivisionExamModel model)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                model.UserId = _userContext.CurrentUser.Id;
                var divisionExam = _smsService.GetDivisionExamMappingById(model.Id);
                divisionExam = model.ToEntity(divisionExam);
                divisionExam.CreatedOn = divisionExam.ModifiedOn = DateTime.Now;
                _smsService.UpdateDivisionExam(divisionExam);
            }
            else
            {
                var examsAlreadyAssociated = _smsService.GetAllExamsByClassDivision(model.DivisionId);
                model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
                {
                    Text = x.ExamName.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ExamId
                }).Where(x => !examsAlreadyAssociated.Any(y => (y.ExamId.ToString() == x.Value)) || model.ExamId.ToString() == x.Value).ToList();

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.AcadmicYearId
                }).ToList();

                var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
                model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
                {
                    Text = x.Number.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ClassRoomId
                }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

                model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                              select new SelectListItem
                                              {
                                                  Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                                  Value = Convert.ToInt32(d).ToString(),
                                                  Selected = false
                                              }).ToList();

                model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                                 select new SelectListItem
                                                 {
                                                     Text = d.ToString(),
                                                     Value = Convert.ToInt32(d).ToString(),
                                                     Selected = false
                                                 }).ToList();
                return View(model);
            }

            SuccessNotification("Division Exam Updated Successfully.");
            return RedirectToAction("EditDivisionExam", new { id=model.Id });
        }

        #endregion

        #region Teacher Exam

        public ActionResult TeacherExams(int id)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var classTeacher = _smsService.GetTeacherById(id);
            if (classTeacher == null)
                throw new ArgumentNullException("classroomteacher");

            var model = new TeacherExamModel();
            model.TeacherId = classTeacher.Id;
            model.Teacher = classTeacher.Name;
            model.UserId = _userContext.CurrentUser.Id;

            var examsAlreadyAssociated = _smsService.GetAllExamsByTeacher(id);
            model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
            {
                Text = x.ExamName.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ExamId
            }).Where(x => !examsAlreadyAssociated.Any(y => y.ExamId.ToString() == x.Value)).ToList();

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.AcadmicYearId
            }).ToList();

            var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
            model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
            {
                Text = x.Number.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ClassRoomId
            }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

            model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                          select new SelectListItem
                                          {
                                              Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                              Value = Convert.ToInt32(d).ToString(),
                                              Selected = false
                                          }).ToList();

            model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                             select new SelectListItem
                                             {
                                                 Text = d.ToString(),
                                                 Value = Convert.ToInt32(d).ToString(),
                                                 Selected = false
                                             }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult TeacherExams(TeacherExamModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var objTeacherExam = model.ToEntity();
                objTeacherExam.Id = 0;
                objTeacherExam.CreatedOn = objTeacherExam.ModifiedOn = DateTime.Now;
                objTeacherExam.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertTeacherExam(objTeacherExam);
            }
            else
            {
                var examsAlreadyAssociated = _smsService.GetAllExamsByTeacher(model.TeacherId);
                model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
                {
                    Text = x.ExamName.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ExamId
                }).Where(x => !examsAlreadyAssociated.Any(y => y.ExamId.ToString() == x.Value)).ToList();

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.AcadmicYearId
                }).ToList();

                var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
                model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
                {
                    Text = x.Number.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ClassRoomId
                }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

                model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                              select new SelectListItem
                                              {
                                                  Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                                  Value = Convert.ToInt32(d).ToString(),
                                                  Selected = model.GradeSystemId == Convert.ToInt32(d)
                                              }).ToList();

                model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                                 select new SelectListItem
                                                 {
                                                     Text = d.ToString(),
                                                     Value = Convert.ToInt32(d).ToString(),
                                                     Selected = model.ResultStatusId == Convert.ToInt32(d)
                                                 }).ToList();
                return View(model);
            }

            SuccessNotification("Exam updated successfully.");
            return RedirectToAction("TeacherExams", new { id = model.Id });
        }


        public ActionResult EditTeacherExam(int id)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var examTeacher = _smsService.GetTeacherExamMappingById(id);
            if (examTeacher == null)
                throw new ArgumentNullException("Exam Teacher");

            var model = examTeacher.ToModel();
            var examsAlreadyAssociated = _smsService.GetAllExamsByTeacher(id);

            model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
            {
                Text = x.ExamName.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ExamId
            }).Where(x => !examsAlreadyAssociated.Any(y => (y.ExamId.ToString() == x.Value)) || model.ExamId.ToString() == x.Value).ToList();

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.AcadmicYearId
            }).ToList();

            var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
            model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
            {
                Text = x.Number.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ClassRoomId
            }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

            model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                          select new SelectListItem
                                          {
                                              Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                              Value = Convert.ToInt32(d).ToString(),
                                              Selected = false
                                          }).ToList();

            model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                             select new SelectListItem
                                             {
                                                 Text = d.ToString(),
                                                 Value = Convert.ToInt32(d).ToString(),
                                                 Selected = false
                                             }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditTeacherExam(TeacherExamModel model)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                model.UserId = _userContext.CurrentUser.Id;
                var teacherExam = _smsService.GetTeacherExamMappingById(model.Id);
                teacherExam = model.ToEntity(teacherExam);
                teacherExam.CreatedOn = teacherExam.ModifiedOn = DateTime.Now;
                _smsService.UpdateTeacherExam(teacherExam);
            }
            else
            {
                var examsAlreadyAssociated = _smsService.GetAllExamsByTeacher(model.TeacherId);
                model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
                {
                    Text = x.ExamName.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ExamId
                }).Where(x => !examsAlreadyAssociated.Any(y => (y.ExamId.ToString() == x.Value)) || model.ExamId.ToString() == x.Value).ToList();

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.AcadmicYearId
                }).ToList();

                var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
                model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
                {
                    Text = x.Number.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ClassRoomId
                }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

                model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                              select new SelectListItem
                                              {
                                                  Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                                  Value = Convert.ToInt32(d).ToString(),
                                                  Selected = false
                                              }).ToList();

                model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                                 select new SelectListItem
                                                 {
                                                     Text = d.ToString(),
                                                     Value = Convert.ToInt32(d).ToString(),
                                                     Selected = false
                                                 }).ToList();
                return View(model);
            }

            SuccessNotification("Teacher Exam Updated Successfully.");
            return RedirectToAction("EditTeacherExam", new { id = model.Id });
        }

        #endregion

        #region Student Exam

        public ActionResult StudentExams(int id)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var classStudent = _smsService.GetStudentById(id);
            if (classStudent == null)
                throw new ArgumentNullException("student");

            var model = new StudentExamModel();
            model.StudentId = classStudent.Id;
            model.Student = classStudent.FName + (!string.IsNullOrEmpty(classStudent.MName) ? (" " + classStudent.MName) : "") + (!string.IsNullOrEmpty(classStudent.LName) ? (" " + classStudent.LName) : "");
            model.UserId = _userContext.CurrentUser.Id;

            var examsAlreadyAssociated = _smsService.GetAllExamsByStudent(id);
            model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
            {
                Text = x.ExamName.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ExamId
            }).Where(x => !examsAlreadyAssociated.Any(y => y.ExamId.ToString() == x.Value)).ToList();

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.AcadmicYearId
            }).ToList();

            var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
            model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
            {
                Text = x.Number.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ClassRoomId
            }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

            model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                          select new SelectListItem
                                          {
                                              Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                              Value = Convert.ToInt32(d).ToString(),
                                              Selected = false
                                          }).ToList();

            model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                             select new SelectListItem
                                             {
                                                 Text = d.ToString(),
                                                 Value = Convert.ToInt32(d).ToString(),
                                                 Selected = false
                                             }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult StudentExams(StudentExamModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var objStudentExam = model.ToEntity();
                objStudentExam.Id = 0;
                objStudentExam.CreatedOn = objStudentExam.ModifiedOn = DateTime.Now;
                objStudentExam.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertStudentExam(objStudentExam);
            }
            else
            {
                var examsAlreadyAssociated = _smsService.GetAllExamsByStudent(model.StudentId);
                model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
                {
                    Text = x.ExamName.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ExamId
                }).Where(x => !examsAlreadyAssociated.Any(y => y.ExamId.ToString() == x.Value)).ToList();

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.AcadmicYearId
                }).ToList();

                var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
                model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
                {
                    Text = x.Number.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ClassRoomId
                }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

                model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                              select new SelectListItem
                                              {
                                                  Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                                  Value = Convert.ToInt32(d).ToString(),
                                                  Selected = model.GradeSystemId == Convert.ToInt32(d)
                                              }).ToList();

                model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                                 select new SelectListItem
                                                 {
                                                     Text = d.ToString(),
                                                     Value = Convert.ToInt32(d).ToString(),
                                                     Selected = model.ResultStatusId == Convert.ToInt32(d)
                                                 }).ToList();
                return View(model);
            }

            SuccessNotification("Exam updated successfully.");
            return RedirectToAction("StudentExams", new { id = model.Id });
        }

        public ActionResult EditStudentExam(int id)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var examStudent = _smsService.GetStudentExamMappingById(id);
            if (examStudent == null)
                throw new ArgumentNullException("Exam Student");

            var model = examStudent.ToModel();
            var examsAlreadyAssociated = _smsService.GetAllExamsByStudent(id);

            model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
            {
                Text = x.ExamName.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ExamId
            }).Where(x => !examsAlreadyAssociated.Any(y => (y.ExamId.ToString() == x.Value)) || model.ExamId.ToString() == x.Value).ToList();

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.AcadmicYearId
            }).ToList();

            var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
            model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
            {
                Text = x.Number.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.ClassRoomId
            }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

            model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                          select new SelectListItem
                                          {
                                              Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                              Value = Convert.ToInt32(d).ToString(),
                                              Selected = false
                                          }).ToList();

            model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                             select new SelectListItem
                                             {
                                                 Text = d.ToString(),
                                                 Value = Convert.ToInt32(d).ToString(),
                                                 Selected = false
                                             }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditStudentExam(StudentExamModel model)
        {
            if (!_permissionService.Authorize("ManageExam"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                model.UserId = _userContext.CurrentUser.Id;
                var studentExam = _smsService.GetStudentExamMappingById(model.Id);
                studentExam = model.ToEntity(studentExam);
                studentExam.CreatedOn = studentExam.ModifiedOn = DateTime.Now;
                _smsService.UpdateStudentExam(studentExam);
            }
            else
            {
                var examsAlreadyAssociated = _smsService.GetAllExamsByStudent(model.StudentId);
                model.AvailableExams = _smsService.GetAllExams().Select(x => new SelectListItem()
                {
                    Text = x.ExamName.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ExamId
                }).Where(x => !examsAlreadyAssociated.Any(y => (y.ExamId.ToString() == x.Value)) || model.ExamId.ToString() == x.Value).ToList();

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.AcadmicYearId
                }).ToList();

                var vacantClassRooms = _smsService.GetVacantClassRoomsForExams();
                model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
                {
                    Text = x.Number.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ClassRoomId
                }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

                model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                              select new SelectListItem
                                              {
                                                  Text = EnumExtensions.GetEnumDescription<GradeSystem>(d),
                                                  Value = Convert.ToInt32(d).ToString(),
                                                  Selected = false
                                              }).ToList();

                model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                                 select new SelectListItem
                                                 {
                                                     Text = d.ToString(),
                                                     Value = Convert.ToInt32(d).ToString(),
                                                     Selected = false
                                                 }).ToList();
                return View(model);
            }

            SuccessNotification("Student Exam Updated Successfully.");
            return RedirectToAction("EditStudentExam", new { id = model.Id });
        }

        #endregion
    }
}
