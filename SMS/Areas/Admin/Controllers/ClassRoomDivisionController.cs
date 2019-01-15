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
    public class ClassRoomDivisionController : AdminAreaController
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
        private readonly IPictureService _pictureService;

        #endregion Fileds

        #region Constructor

        public ClassRoomDivisionController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService, IPictureService pictureService)
        {
            this._userService = userService;
            this._userContext = userContext;
            this._settingService = settingService;
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._smsService = smsService;
            this._commentService = commentService;
            this._replyService = replyService;
            this._pictureService = pictureService;
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
                var divisionData = (from tempdivisions in _smsService.GetAllClassRoomDivisions() select tempdivisions);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    divisionData = divisionData.Where(m => m.Class.Name.Contains(searchValue) || m.Division.Name.Contains(searchValue) || m.ClassRoom.Number.Contains(searchValue));
                }

                //total number of rows count     
                recordsTotal = divisionData.Count();
                //Paging     
                var data = divisionData.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new ClassRoomDivisionModel()
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

        [HttpPost]
        public ActionResult LoadSubjectGrid(int id)
        {
            try
            {
                var subjectData = (from associatedsubject in _smsService.GetAllSubjectsByDivision(id) select associatedsubject).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = subjectData.Select(x => new DivisionSubjectModel()
                        {
                            Id = x.Id,
                            SubjectName = x.Subject.Name.Trim(),
                            SubjectCode = x.Subject.Code.Trim(),
                            ClassName = x.Division.Class.Name,
                            ClassId = x.Division.ClassId.Value,
                            DivisionId = x.DivisionId,
                            DivisionName = x.Division.Division.Name,
                            SubjectId = x.SubjectId
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
        public ActionResult LoadHomeworkGrid(int id)
        {
            try
            {
                var homeworkData = (from associatedhomework in _smsService.GetAllHomeworksByDivision(id) select associatedhomework).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = homeworkData.Select(x => new DivisionHomeworkModel()
                        {
                            Id = x.Id,
                            Class = x.Division.Class.Name,
                            Division = x.Division.Division.Name,
                            ClassId = x.Division.ClassId.Value,
                            DivisionId = x.DivisionId,
                            Homework = x.Homework.Name,
                            HomeworkId = x.HomeworkId,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate
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
        public ActionResult LoadExamGrid(int id)
        {
            try
            {
                var examData = (from associatedexam in _smsService.GetAllExamsByDivision(id) select associatedexam).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = examData.Select(x => new DivisionExamModel()
                        {
                            Id = x.Id,
                            Class = x.Division.Class.Name,
                            Division = x.Division.Division.Name,
                            ClassId = x.Division.ClassId.Value,
                            DivisionId = x.DivisionId,
                            Exam = x.Exam.ExamName,
                            ExamId = x.ExamId,
                            BreakAllowed = x.BreakAllowed,
                            MarksObtained = x.MarksObtained,
                            AcadmicYear = _smsService.GetAcadmicYearById(x.Exam.AcadmicYearId).Name,
                            EndDate = x.EndDate,
                            StartDate = x.StartDate,
                            GradeSystemId = x.GradeSystemId,
                            GradeSystem = x.GradeSystemId.HasValue && x.GradeSystemId.Value > 0 ? EnumExtensions.GetDescriptionByValue<GradeSystem>(x.GradeSystemId.Value) : "",
                            ClassRoom = _smsService.GetClassRoomById(x.ClassRoomId).Number
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
        public ActionResult LoadStudentGrid(int id)
        {
            try
            {
                var studentData = (from associatedstudent in _smsService.GetStudentsByDivision(id) select associatedstudent).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = studentData.Select(x => new StudentModel()
                        {
                            Id = x.Id,
                            PictureSrc = x.StudentPictureId > 0 ? _pictureService.GetPictureById(x.StudentPictureId).PictureSrc : "",
                            FName = x.FName,
                            MName = x.MName,
                            LName = x.LName,
                            FatherFName = x.FatherFName,
                            FatherLName = x.FatherLName,
                            AdmissionDate = x.AdmissionDate,
                            DateOfBirth = x.DateOfBirth
                        }).OrderByDescending(x => x.AdmissionDate).ThenBy(x => x.FName).ToList()
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

        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            var model = _smsService.GetAllClasses().Select(x => new AllotClassRoomsToClass()
            {
                AvailableDivisions = _smsService.GetAllDivisionsByClass(x.Id).Select(y => new DivisionModel()
                {
                    Name = y.Division.Name,
                    Id = y.DivisionId.Value,
                    AvailableClassRooms = _smsService.GetAllClassRooms().Select(z => new SelectListItem()
                    {
                        Text = z.Number,
                        Value = z.Id.ToString(),
                        Selected = y.ClassRoomId.HasValue && y.ClassRoomId.Value == z.Id
                    }).Where(z => ((y.ClassRoomId.HasValue && y.ClassRoomId.Value.ToString().Trim() == z.Value.Trim()) || (!_smsService.CheckClassRoomAlreadyAssociatedToOtherDivisionAndClass(classroomid: Convert.ToInt32(z.Value), classid: x.Id, divisionid: y.DivisionId)))).ToList()
                }).ToList(),
                Class = x.Name,
                ClassId = x.Id
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult List(IList<AllotClassRoomsToClass> model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            var classRoomUpdations = frm.AllKeys.Where(pm => pm.StartsWith("classroom_")).ToList();
            if (classRoomUpdations.Count > 0)
            {
                int count = 0;
                int classid = 0;
                int divisionid = 0;
                foreach (var key in classRoomUpdations)
                {
                    count += 1;
                    classid = Convert.ToInt32(key.Split('_')[1]);
                    divisionid = Convert.ToInt32(key.Split('_')[2]);
                    if (classid > 0 && divisionid > 0 && frm["classroom_" + classid + "_" + divisionid] != null && !string.IsNullOrEmpty(frm["classroom_" + classid + "_" + divisionid].ToString()))
                    {
                        var selectedValue = Convert.ToInt32(frm["classroom_" + classid + "_" + divisionid].ToString());
                        if (selectedValue > 0)
                        {
                            var selectedClassroomDivision = _smsService.GetClassDivisions(classid: classid, divisionid: divisionid).FirstOrDefault();
                            if (selectedClassroomDivision != null)
                            {
                                selectedClassroomDivision.ClassRoomId = selectedValue;
                                selectedClassroomDivision.ModifiedOn = DateTime.Now;
                                _smsService.UpdateClassDivision(selectedClassroomDivision);
                            }
                            else
                            {
                                selectedClassroomDivision = new ClassRoomDivision();
                                selectedClassroomDivision.ClassId = classid;
                                selectedClassroomDivision.DivisionId = divisionid;
                                selectedClassroomDivision.ClassRoomId = selectedValue;
                                selectedClassroomDivision.CreatedOn = selectedClassroomDivision.ModifiedOn = DateTime.Now;
                                selectedClassroomDivision.UserId = _userContext.CurrentUser.Id;
                                _smsService.InsertClassDivision(selectedClassroomDivision);
                            }
                        }
                    }
                }
            }

            SuccessNotification("Class Rooms updated successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Divisions()
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            var model = new ClassRoomDivisionModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Division Id Missing");

            var classRoomDivision = _smsService.GetClassRoomDivisionById(id);
            var model = classRoomDivision.ToModel();

            model.Class = _smsService.GetClassById(model.ClassId).Name;
            model.Division = _smsService.GetDivisionById(model.DivisionId).Name;
            model.ClassRoom = _smsService.GetClassRoomById(model.ClassRoomId).Number;
            return View(model);
        }

        #region Subject Association

        public JsonResult GetAllSubjectsByDivision(int divisionid)
        {
            var allSubjects = _smsService.GetAllSubjects();
            var subjectsByDivision = _smsService.GetAllSubjectsByDivision(id: divisionid);

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
                    Selected = subjectsByDivision.Any(y => y.Id == x.Id)
                }).OrderBy(x => x.Code).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        public ActionResult UpdateSubjectsForDivision(int id, int[] subjects)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var user = _userContext.CurrentUser;
            var objDivision = _smsService.GetClassRoomDivisionById(id);
            if (objDivision != null)
            {
                var objSubjects = _smsService.GetAllSubjectsByDivision(id);
                if (subjects != null && subjects.Length > 0)
                {
                    foreach (var subjectid in subjects)
                    {
                        var checkRecords = objDivision.Subjects.Any(x => x.Id == subjectid);
                        if (!checkRecords)
                        {
                            _smsService.InsertDivisionSubject(new DivisionSubject()
                            {
                                SubjectId = id,
                                DivisionId = subjectid,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                UserId = user.Id
                            });
                        }
                    }
                }
                else
                {
                    foreach (var record in objSubjects)
                    {
                        var objDivisionSubjects = _smsService.GetDivisionSubjects(divisionid: record.DivisionId, subjectid: record.SubjectId);
                        if (objDivisionSubjects != null && objDivisionSubjects.Count > 0)
                        {
                            foreach (var divsubject in objDivisionSubjects)
                            {
                                _smsService.RemoveSubjectFromDivision(id, divsubject.SubjectId);
                            }
                        }
                    }
                }
            }
            ViewBag.Result = "Division updated Successfully";
            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult RemoveSubjectFromDivision(int id, int subjectid)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Division id not found");

            var objDivision = _smsService.GetClassRoomDivisionById(id);
            if (objDivision != null)
            {
                var selectSubject = objDivision.Subjects.FirstOrDefault(x => x.Id == subjectid);
                if (selectSubject != null)
                {
                    objDivision.Subjects.Remove(selectSubject);
                    _smsService.UpdateClassDivision(objDivision);
                }
            }

            SuccessNotification("Subject removed successfully from selected division.");
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

        #region Homework Association

        public JsonResult GetAllHomeworksByDivision(int divisionid)
        {
            var allHomeworks = _smsService.GetAllHomeworks();
            var homeworksByDivision = _smsService.GetAllHomeworksByDivision(id: divisionid);

            //Returning Json Data 
            return new JsonResult()
            {
                Data = allHomeworks.Select(x => new HomeworkModel()
                {
                    UserId = x.UserId,
                    Id = x.Id,
                    AcadmicYearId = x.AcadmicYearId,
                    Name = x.Name,
                    StartDate = homeworksByDivision.Any(y => y.HomeworkId == x.Id) ? homeworksByDivision.FirstOrDefault(y => y.HomeworkId == x.Id).StartDate : null,
                    EndDate = homeworksByDivision.Any(y => y.HomeworkId == x.Id) ? homeworksByDivision.FirstOrDefault(y => y.HomeworkId == x.Id).EndDate : null,
                    CreatedOnString = x.CreatedOn.ToString("U"),
                    ModifiedOnString = x.ModifiedOn.ToString("U"),
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Selected = homeworksByDivision.Any(y => y.Id == x.Id)
                }).OrderBy(x => x.StartDate).ThenBy(x => x.EndDate).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        public ActionResult UpdateHomeworksForDivision(int id, DivisionHomeworkModel[] homeworks)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var user = _userContext.CurrentUser;
            var objDivision = _smsService.GetClassRoomDivisionById(id);
            if (objDivision != null)
            {
                var objDivisions = _smsService.GetAllHomeworksByDivision(id);
                if (homeworks != null && homeworks.Length > 0)
                {
                    foreach (var hwork in homeworks)
                    {
                        var checkHomework = objDivision.DivisionHomeworks.FirstOrDefault(x => x.HomeworkId == hwork.HomeworkId && x.DivisionId == id);
                        if (checkHomework == null)
                        {
                            _smsService.InsertDivisionHomework(new DivisionHomework()
                            {
                                DivisionId = hwork.DivisionId,
                                EndDate = hwork.EndDate,
                                HomeworkId = hwork.DivisionId,
                                StartDate = hwork.StartDate,
                                StudentHomeworkStatusId = hwork.StudentHomeworkStatusId,
                                TeacherApprovalStatusId = hwork.TeacherApprovalStatusId,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                UserId = user.Id
                            });
                        }
                        else
                        {
                            checkHomework.EndDate = hwork.EndDate;
                            checkHomework.StartDate = hwork.StartDate;
                            checkHomework.StudentHomeworkStatusId = hwork.StudentHomeworkStatusId;
                            checkHomework.TeacherApprovalStatusId = hwork.TeacherApprovalStatusId;
                            checkHomework.ModifiedOn = DateTime.Now;
                            _smsService.UpdateDivisionHomework(checkHomework);
                        }
                    }
                }
                else
                {
                    foreach (var record in objDivisions)
                    {
                        var objDivisionHomeworks = _smsService.GetDivisionHomeworks(divisionid: record.DivisionId, homeworkid: record.HomeworkId);
                        if (objDivisionHomeworks != null && objDivisionHomeworks.Count > 0)
                        {
                            foreach (var divhomework in objDivisionHomeworks)
                            {
                                _smsService.RemoveHomeworkFromDivision(id, divhomework.HomeworkId);
                            }
                        }
                    }
                }
            }
            ViewBag.Result = "Division updated Successfully";
            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult RemoveHomeworkFromDivision(int id, int homeworkid)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Division id not found");

            var objDivision = _smsService.GetClassRoomDivisionById(id);
            if (objDivision != null)
            {
                var selectHomework = objDivision.DivisionHomeworks.FirstOrDefault(x => x.HomeworkId == homeworkid && x.DivisionId == id);
                if (selectHomework != null)
                {
                    objDivision.DivisionHomeworks.Remove(selectHomework);
                    _smsService.UpdateClassDivision(objDivision);
                }
            }

            SuccessNotification("Homework removed successfully from selected division.");
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

        #region Exam Association

        public JsonResult GetAllExamsByDivision(int divisionid)
        {
            var allExams = _smsService.GetAllDivisionExamMappings();
            var examsByDivision = _smsService.GetAllExamsByDivision(id: divisionid);

            //Returning Json Data 
            return new JsonResult()
            {
                Data = allExams.Select(x => new DivisionExamModel()
                {
                    Id = x.Id,
                    BreakTime = x.BreakTime,
                    ClassId = x.Division.ClassId.Value,
                    ClassRoomId = x.ClassRoomId,
                    ClassRoom = _smsService.GetClassRoomById(x.ClassRoomId).Number,
                    Class = _smsService.GetClassById(x.Division.ClassId.Value).Name,
                    DivisionId = x.DivisionId,
                    Division = x.DivisionId > 0 ? _smsService.GetDivisionById(x.DivisionId).Name : "",
                    BreakAllowed = x.BreakAllowed,
                    EndDate = x.EndDate,
                    Exam = _smsService.GetExamById(x.ExamId).ExamName,
                    GradeSystemId = x.GradeSystemId,
                    ExamId = x.ExamId,
                    AcadmicYearId = x.Exam.AcadmicYearId,
                    AcadmicYear = _smsService.GetAcadmicYearById(x.Exam.AcadmicYearId).Name,
                    MarksObtained = x.MarksObtained,
                    ResultStatusId = x.ResultStatusId,
                    StartDate = x.StartDate,
                    UserId = x.UserId,
                    AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(y => new SelectListItem()
                    {
                        Text = y.Name.Trim(),
                        Value = y.Id.ToString(),
                        Selected = x.Exam != null ? y.Id == x.Exam.AcadmicYearId : y.IsActive
                    }).ToList(),
                    AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                            select new SelectListItem
                                            {
                                                Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
                                                Value = Convert.ToInt32(d).ToString(),
                                                Selected = (Convert.ToInt32(d) == x.GradeSystemId)
                                            }).ToList(),
                    AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                            select new SelectListItem
                                            {
                                                Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
                                                Value = Convert.ToInt32(d).ToString(),
                                                Selected = (Convert.ToInt32(d) == x.ResultStatusId)
                                            }).ToList(),
                    Selected = examsByDivision.Any(y => y.Id == x.Id)
                }).OrderBy(x => x.StartDate).ThenBy(x => x.EndDate).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        public ActionResult UpdateExamsForDivision(int id, DivisionExamModel[] exams)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var user = _userContext.CurrentUser;
            var objDivision = _smsService.GetClassRoomDivisionById(id);
            if (objDivision != null)
            {
                var objExams = _smsService.GetAllExamsByDivision(id);
                if (exams != null && exams.Length > 0)
                {
                    foreach (var ex in exams)
                    {
                        var checkExam = objDivision.DivisionExams.FirstOrDefault(x => x.ExamId == ex.ExamId && x.DivisionId == ex.DivisionId);
                        if (checkExam == null)
                        {
                            _smsService.InsertDivisionExam(new DivisionExam()
                            {
                                BreakAllowed = ex.BreakAllowed,
                                BreakTime = ex.BreakTime,
                                ClassRoomId = ex.ClassRoomId,
                                DivisionId = ex.DivisionId,
                                EndDate = ex.EndDate,
                                EndTime = ex.EndTime,
                                ExamId = ex.ExamId,
                                StartDate = ex.StartDate,
                                StartTime = ex.StartTime,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                UserId = user.Id
                            });
                        }
                        else
                        {
                            checkExam.BreakAllowed = ex.BreakAllowed;
                            checkExam.BreakTime = ex.BreakTime;
                            checkExam.ClassRoomId = ex.ClassRoomId;
                            checkExam.DivisionId = ex.DivisionId;
                            checkExam.EndDate = ex.EndDate;
                            checkExam.EndTime = ex.EndTime;
                            checkExam.ExamId = ex.ExamId;
                            checkExam.StartDate = ex.StartDate;
                            checkExam.StartTime = ex.StartTime;
                            checkExam.ModifiedOn = DateTime.Now;
                            _smsService.UpdateDivisionExam(checkExam);
                        }
                    }
                }
                else
                {
                    foreach (var record in objExams)
                    {
                        var objDivisionExams = _smsService.GetDivisionExams(divisionid: record.DivisionId, examid: record.ExamId);
                        if (objDivisionExams != null && objDivisionExams.Count > 0)
                        {
                            foreach (var divexam in objDivisionExams)
                            {
                                _smsService.RemoveExamFromDivision(id, divexam.ExamId);
                            }
                        }
                    }
                }
            }
            ViewBag.Result = "Division updated Successfully";
            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult RemoveExamFromDivision(int id, int examid)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Division id not found");

            var objDivision = _smsService.GetClassRoomDivisionById(id);
            if (objDivision != null)
            {
                var selectExam = objDivision.DivisionExams.FirstOrDefault(x => x.ExamId == examid && x.DivisionId == id);
                if (selectExam != null)
                {
                    objDivision.DivisionExams.Remove(selectExam);
                    _smsService.UpdateClassDivision(objDivision);
                }
            }

            SuccessNotification("Exam removed successfully from selected division.");
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

        #region Student Association

        public JsonResult GetAllStudentsByDivision(int divisionid)
        {
            var studentsByDivision = _smsService.GetStudentsByDivision(id: divisionid);

            //Returning Json Data 
            return new JsonResult()
            {
                Data = studentsByDivision.Select(x => new StudentModel()
                {
                    Id = x.Id,
                    PictureSrc = x.StudentPictureId > 0 ? _pictureService.GetPictureById(x.StudentPictureId).PictureSrc : "",
                    FName = x.FName,
                    MName = x.MName,
                    LName = x.LName,
                    FatherFName = x.FatherFName,
                    FatherLName = x.FatherLName,
                    AdmissionDate = x.AdmissionDate,
                    DateOfBirth = x.DateOfBirth
                }).OrderByDescending(x => x.AdmissionDate).ThenBy(x => x.FName).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion

    }
}
