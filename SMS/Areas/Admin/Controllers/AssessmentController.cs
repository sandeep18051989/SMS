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
using MVCEncrypt;
using EF.Services.Http;

namespace SMS.Areas.Admin.Controllers
{
    public class AssessmentController : AdminAreaController
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
        private readonly IUrlHelper _urlHelper;

        #endregion Fileds

        #region Constructor

        public AssessmentController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService, IUrlHelper urlHelper)
        {
            this._userService = userService;
            this._userContext = userContext;
            this._settingService = settingService;
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._smsService = smsService;
            this._commentService = commentService;
            this._replyService = replyService;
            this._urlHelper = urlHelper;
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
                var assessmentData = (from tempassessments in _smsService.GetAllAssessments() select tempassessments);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    assessmentData = assessmentData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstAssessments = assessmentData as Assessment[] ?? assessmentData.ToArray();
                recordsTotal = lstAssessments.Count();
                //Paging     
                var data = lstAssessments.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new AssessmentModel()
                        {
                            Name = x.Name,
                            AcadmicYearId = x.AcadmicYearId,
                            AllowUserToMoveForwardBackward = x.AllowUserToMoveForwardBackward,
                            DifficultyLevelId = x.DifficultyLevelId,
                            DifficultyLevel = x.DifficultyLevelId > 0 ? EnumExtensions.GetDescriptionByValue<DifficultyLevel>(x.DifficultyLevelId) : "",
                            CreatedOn = x.CreatedOn,
                            DurationInMinutes = x.DurationInMinutes,
                            EndTime = x.EndTime,
                            Instructions = x.Instructions,
                            IsTimeBound = x.IsTimeBound,
                            LogoPictureId = x.LogoPictureId,
                            MandatoryToSolveAll = x.MandatoryToSolveAll,
                            MessageOnSubmitTest = x.MessageOnSubmitTest,
                            OpenToAnonymousUsers = x.OpenToAnonymousUsers,
                            ShowResultToCandidate = x.ShowResultToCandidate,
                            SignaturePictureId = x.SignaturePictureId,
                            StartTime = x.StartTime,
                            TotalQuestions = x.TotalQuestions,
                            Url = x.Url,
                            UserId = x.UserId,
                            PassingMarks = x.PassingMarks,
                            MaxMarks = x.MaxMarks,
                            IsActive = x.IsActive,
                            Id = x.Id,
                            StringStartTime = x.StartTime != null ? x.StartTime.Value.ToString("dd MMMM yyyy HH:mm tt") : "",
                            StringEndTime = x.EndTime != null ? x.EndTime.Value.ToString("dd MMMM yyyy HH:mm tt") : ""
                        }).OrderBy(x => x.Name).ToList()
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

        public JsonResult GetAllAssessmentsByStudent(int studentid)
        {
            var allAssessments = _smsService.GetAllAssessments();
            var assessmentByClassDivision = _smsService.GetAllAssessmentsByStudent(studentid);

            //Returning Json Data 
            return new JsonResult()
            {
                Data = allAssessments.Select(x => new AssessmentModel()
                {
                    Name = x.Name,
                    AcadmicYearId = x.AcadmicYearId,
                    AllowUserToMoveForwardBackward = x.AllowUserToMoveForwardBackward,
                    DifficultyLevelId = x.DifficultyLevelId,
                    DifficultyLevel = x.DifficultyLevelId > 0 ? EnumExtensions.GetDescriptionByValue<DifficultyLevel>(x.DifficultyLevelId) : "",
                    CreatedOn = x.CreatedOn,
                    DurationInMinutes = x.DurationInMinutes,
                    EndTime = x.EndTime,
                    Instructions = x.Instructions,
                    IsTimeBound = x.IsTimeBound,
                    LogoPictureId = x.LogoPictureId,
                    MandatoryToSolveAll = x.MandatoryToSolveAll,
                    MessageOnSubmitTest = x.MessageOnSubmitTest,
                    OpenToAnonymousUsers = x.OpenToAnonymousUsers,
                    ShowResultToCandidate = x.ShowResultToCandidate,
                    SignaturePictureId = x.SignaturePictureId,
                    StartTime = x.StartTime,
                    TotalQuestions = x.TotalQuestions,
                    Url = x.Url,
                    UserId = x.UserId,
                    PassingMarks = x.PassingMarks,
                    MaxMarks = x.MaxMarks,
                    IsActive = x.IsActive,
                    Id = x.Id,
                    StringStartTime = x.StartTime != null ? x.StartTime.Value.ToString("dd MMMM yyyy HH:mm tt") : "",
                    StringEndTime = x.EndTime != null ? x.EndTime.Value.ToString("dd MMMM yyyy HH:mm tt") : ""
                }).OrderBy(x => x.Name).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        public JsonResult CheckAssessmentNameExists(string name, int? id)
        {
            return new JsonResult()
            {
                Data = _smsService.CheckAssessmentExists(name, id),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion
        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            var model = new AssessmentModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new AssessmentModel();
            var objAssessment = _smsService.GetAssessmentById(id);
            if (objAssessment != null)
            {
                model = objAssessment.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.AcadmicYearId == x.Id
            }).ToList();

            model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim() + "(" + x.Code + ")",
                Value = x.Id.ToString(),
                Selected = model.SubjectId > 0 ? model.SubjectId == x.Id : false
            }).ToList();

            model.AvailableDifficultyLevels = (from DifficultyLevel d in Enum.GetValues(typeof(DifficultyLevel))
                                               select new SelectListItem
                                               {
                                                   Text = EnumExtensions.GetDescriptionByValue<DifficultyLevel>(Convert.ToInt32(d)),
                                                   Value = Convert.ToInt32(d).ToString(),
                                                   Selected = (Convert.ToInt32(d) == model.DifficultyLevelId)
                                               }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(AssessmentModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            // Check for duplicate assessment, if any
            var checkAssessment = _smsService.CheckAssessmentExists(model.Name, model.Id);
            if (checkAssessment)
                ModelState.AddModelError("Name", "An Assessment with the same name already exists.");

            if (ModelState.IsValid)
            {
                var objAssessment = _smsService.GetAssessmentById(model.Id);
                if (objAssessment != null)
                {
                    objAssessment = model.ToEntity(objAssessment);
                    objAssessment.ModifiedOn = DateTime.Now;
                    objAssessment.UserId = _userContext.CurrentUser.Id;
                    _smsService.UpdateAssessment(objAssessment);
                }
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId == x.Id
                }).ToList();

                model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim() + "(" + x.Code + ")",
                    Value = x.Id.ToString(),
                    Selected = model.SubjectId > 0 ? model.SubjectId == x.Id : false
                }).ToList();

                model.AvailableDifficultyLevels = (from DifficultyLevel d in Enum.GetValues(typeof(DifficultyLevel))
                                                   select new SelectListItem
                                                   {
                                                       Text = EnumExtensions.GetDescriptionByValue<DifficultyLevel>(Convert.ToInt32(d)),
                                                       Value = Convert.ToInt32(d).ToString(),
                                                       Selected = (Convert.ToInt32(d) == model.DifficultyLevelId)
                                                   }).ToList();
                return View(model);
            }

            SuccessNotification("Assessment updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            var model = new AssessmentModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();

            model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim() + "(" + x.Code + ")",
                Value = x.Id.ToString()
            }).ToList();

            model.AvailableDifficultyLevels = (from DifficultyLevel d in Enum.GetValues(typeof(DifficultyLevel))
                                               select new SelectListItem
                                               {
                                                   Text = EnumExtensions.GetDescriptionByValue<DifficultyLevel>(Convert.ToInt32(d)),
                                                   Value = Convert.ToInt32(d).ToString(),
                                                   Selected = false
                                               }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(AssessmentModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            // Check for duplicate assessment, if any
            var checkAssessment = _smsService.CheckAssessmentExists(model.Name, model.Id);
            if (checkAssessment)
                ModelState.AddModelError("Name", "An Assessment with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objAssessment = model.ToEntity();
                objAssessment.CreatedOn = objAssessment.ModifiedOn = DateTime.Now;
                objAssessment.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertAssessment(objAssessment);

                SuccessNotification("Assessment created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objAssessment.Id });
                }
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId == x.Id
                }).ToList();

                model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim() + "(" + x.Code + ")",
                    Value = x.Id.ToString(),
                    Selected = model.SubjectId > 0 ? model.SubjectId == x.Id : false
                }).ToList();

                model.AvailableDifficultyLevels = (from DifficultyLevel d in Enum.GetValues(typeof(DifficultyLevel))
                                                   select new SelectListItem
                                                   {
                                                       Text = EnumExtensions.GetDescriptionByValue<DifficultyLevel>(Convert.ToInt32(d)),
                                                       Value = Convert.ToInt32(d).ToString(),
                                                       Selected = (Convert.ToInt32(d) == model.DifficultyLevelId)
                                                   }).ToList();
                return View(model);
            }

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteAssessment(id);

            SuccessNotification("Assessment deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusAssessment(id);
            ViewBag.Result = "Assessment updated Successfully";

            return Json(new { Result = true });
        }

        #region Question Assessment

        public ActionResult AssessmentQuestions(int id)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var assessment = _smsService.GetAssessmentById(id);
            if (assessment == null)
                throw new ArgumentNullException("id");

            var model = new AssignQuestionsModel();
            model.Assessment = assessment.Name;
            model.AssessmentId = assessment.Id;
            model.DifficultyLevel = assessment.DifficultyLevelId > 0 ? EnumExtensions.GetDescriptionByValue<DifficultyLevel>(assessment.DifficultyLevelId) : "";
            model.IsTimeBound = assessment.IsTimeBound;
            model.MaxMarks = assessment.MaxMarks;
            model.PassingMarks = assessment.PassingMarks;
            model.StringStartTime = assessment.StartTime.Value.ToString("MMMM dd, yyyy HH:mm tt");
            model.StringEndTime = assessment.EndTime.Value.ToString("MMMM dd, yyyy HH:mm tt");
            model.Subject = assessment.SubjectId.HasValue && assessment.SubjectId.Value > 0 ? _smsService.GetSubjectById(assessment.SubjectId.Value).Name : "-";
            model.TotalQuestions = assessment.TotalQuestions;
            model.Url = assessment.Url;

            var questionsAssociated = _smsService.GetQuestionsByAssessmentId(id);
            // Subjects
            var pSubjects = new List<int>();

            if (assessment.SubjectId.HasValue)
                pSubjects.Add(assessment.SubjectId.Value);

            var allquestions = _smsService.SearchQuestions(subjectids: pSubjects.ToArray(), difficultylevel: (assessment.DifficultyLevelId), onlytimebound: assessment.IsTimeBound);
            foreach (var q in allquestions)
            {
                model.List.Add(new AssessmentQuestionModel()
                {
                    AssessmentId = id,
                    Assessment = assessment.Name,
                    DisplayOrder = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).DisplayOrder : 0),
                    Id = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).Id : 0),
                    IsTimeBound = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).IsTimeBound : q.IsTimeBound),
                    NegativeMarks = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).NegativeMarks : q.NegativeMarks),
                    Question = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).Question.Name : q.Name),
                    QuestionId = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).QuestionId : q.Id),
                    RightMarks = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).RightMarks : q.RightMarks),
                    SolveTime = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).SolveTime : q.SolveTime),
                    UserId = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).UserId : q.UserId),
                    IsChecked = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? true : false)
                });
            }

            model.List = model.List.OrderBy(x => x.DisplayOrder).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AssessmentQuestions(AssignQuestionsModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            if (ModelState.IsValid && model.SelectedQuestion != null && model.SelectedQuestion.Length > 0)
            {
                var alreadyAddedQuestions = _smsService.GetQuestionsByAssessmentId(model.AssessmentId);
                foreach (var id in model.SelectedQuestion)
                {
                    var aQuestion = new AssessmentQuestion();
                    aQuestion.QuestionId = id;
                    aQuestion.AssessmentId = model.AssessmentId;
                    aQuestion.RightMarks = (frm["RightMarks_" + id] != null && !string.IsNullOrEmpty(frm["RightMarks_" + id].ToString())) ? Convert.ToDouble(frm["RightMarks_" + id].ToString()) : 0;
                    aQuestion.NegativeMarks = (frm["NegativeMarks_" + id] != null && !string.IsNullOrEmpty(frm["NegativeMarks_" + id].ToString())) ? Convert.ToDouble(frm["NegativeMarks_" + id].ToString()) : 0;
                    aQuestion.DisplayOrder = (frm["DisplayOrder_" + id] != null && !string.IsNullOrEmpty(frm["DisplayOrder_" + id].ToString())) ? Convert.ToInt32(frm["DisplayOrder_" + id].ToString()) : 0;
                    aQuestion.SolveTime = (frm["SolveTime_" + id] != null && !string.IsNullOrEmpty(frm["SolveTime_" + id].ToString())) ? frm["SolveTime_" + id].ToString() : "";
                    aQuestion.IsTimeBound = (frm["IsTimeBound_" + id] != null && !string.IsNullOrEmpty(frm["IsTimeBound_" + id].ToString())) ? Convert.ToBoolean(frm["IsTimeBound_" + id].ToString()) : false;

                    if (alreadyAddedQuestions.Any(x => x.QuestionId == id))
                    {
                        var assessmentQuestion = alreadyAddedQuestions.FirstOrDefault(x => x.QuestionId == id);
                        assessmentQuestion.RightMarks = aQuestion.RightMarks;
                        assessmentQuestion.NegativeMarks = aQuestion.NegativeMarks;
                        assessmentQuestion.DisplayOrder = aQuestion.DisplayOrder;
                        assessmentQuestion.SolveTime = aQuestion.SolveTime;
                        assessmentQuestion.IsTimeBound = aQuestion.IsTimeBound;
                        assessmentQuestion.ModifiedOn = DateTime.Now;
                        _smsService.UpdateAssessmentQuestion(assessmentQuestion);
                    }
                    else
                    {
                        aQuestion.CreatedOn = aQuestion.ModifiedOn = DateTime.Now;
                        aQuestion.UserId = _userContext.CurrentUser.Id;
                        _smsService.InsertAssessmentQuestion(aQuestion);
                    }
                }

                if (alreadyAddedQuestions.Count > 0)
                {
                    var removeQuestions = alreadyAddedQuestions.Where(y => model.SelectedQuestion.Any(z => z != y.QuestionId)).ToList();
                    foreach (var q in removeQuestions)
                    {
                        _smsService.DeleteAssessmentQuestion(q.QuestionId);
                    }
                }
            }
            SuccessNotification("Assessment updated successfully.");
            return RedirectToAction("AssessmentQuestions", new { id = model.AssessmentId });
        }

        #endregion

        #region Student Assessment

        public ActionResult AssessmentStudents(int id)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var assessment = _smsService.GetAssessmentById(id);
            if (assessment == null)
                throw new ArgumentNullException("id");

            var model = new AssignStudentsModel();
            model.Assessment = assessment.Name;
            model.AssessmentId = assessment.Id;
            model.DifficultyLevel = assessment.DifficultyLevelId > 0 ? EnumExtensions.GetDescriptionByValue<DifficultyLevel>(assessment.DifficultyLevelId) : "";
            model.IsTimeBound = assessment.IsTimeBound;
            model.MaxMarks = assessment.MaxMarks;
            model.PassingMarks = assessment.PassingMarks;
            model.StringStartTime = assessment.StartTime.Value.ToString("MMMM dd, yyyy HH:mm tt");
            model.StringEndTime = assessment.EndTime.Value.ToString("MMMM dd, yyyy HH:mm tt");
            model.Subject = assessment.SubjectId.HasValue && assessment.SubjectId.Value > 0 ? _smsService.GetSubjectById(assessment.SubjectId.Value).Name : "-";
            model.Url = assessment.Url;
            model.Duration = assessment.DurationInMinutes;
            model.StartTime = assessment.StartTime.Value;
            model.EndTime = assessment.EndTime.Value;

            var allHolidays = _smsService.GetAllHolidaysByAcadmicYear(assessment.AcadmicYearId);
            model.Holidays = allHolidays.Select(x => x.Date.Value.ToString("MMMM dd, yyyy")).ToArray();

            var studentsAssociated = _smsService.GetStudentsByAssessmentId(id);
            // Subjects
            var pSubjects = new List<int>();

            if (assessment.SubjectId.HasValue)
                pSubjects.Add(assessment.SubjectId.Value);

            var allstudents = _smsService.SearchStudents();
            foreach (var q in allstudents)
            {
                model.List.Add(new AssessmentStudentModel()
                {
                    AssessmentId = id,
                    Assessment = assessment.Name,
                    AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                            select new SelectListItem
                                            {
                                                Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
                                                Value = Convert.ToInt32(d).ToString(),
                                                Selected = false
                                            }).ToList(),
                    AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                               select new SelectListItem
                                               {
                                                   Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
                                                   Value = Convert.ToInt32(d).ToString(),
                                                   Selected = false
                                               }).ToList(),
                    Id = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Id : 0),
                    CertificateHtml = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).CertificateHtml : ""),
                    GradeSystemId = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).GradeSystemId : 0),
                    IsCompleted = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).IsCompleted : false),
                    StudentId = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).StudentId : q.Id),
                    IsActive = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).IsActive : false),
                    IsExpired = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).IsExpired : false),
                    MarksObtained = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).MarksObtained : 0),
                    ResultStatusId = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).ResultStatusId : 0),
                    Url = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Url : ""),
                    UserId = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).UserId : q.UserId),
                    IsChecked = (studentsAssociated.Any(x => x.StudentId == q.Id) ? true : false),
                    StartOn = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).StartOn : assessment.StartTime),
                    EndOn = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).EndOn : assessment.EndTime),
                    Student = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Student.FName + (!string.IsNullOrEmpty(studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Student.LName) ? (" " + studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Student.LName) : "") : _smsService.GetStudentById(q.Id).FName + (!string.IsNullOrEmpty(_smsService.GetStudentById(q.Id).LName) ? (" " + _smsService.GetStudentById(q.Id).LName) : ""))
                });
            }

            model.AvailableClassDivisions = _smsService.GetAllClassRoomDivisions().Select(x => new SelectListItem()
            {
                Text = "Class-" + _smsService.GetClassById(x.ClassId.Value).Name + ", Division-" + _smsService.GetDivisionById(x.DivisionId.Value).Name,
                Value = x.Id.ToString(),
                Selected = model.ClassDivisionId == x.Id
            }).OrderBy(x => x.Text).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("continue-selection", "continueSelection")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AssessmentStudents(AssignStudentsModel model, FormCollection frm, bool continueSelection)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            if (continueSelection)
            {
                model.AvailableClassDivisions = _smsService.GetAllClassRoomDivisions().Select(x => new SelectListItem()
                {
                    Text = "Class-" + _smsService.GetClassById(x.ClassId.Value).Name + ", Division-" + _smsService.GetDivisionById(x.DivisionId.Value).Name,
                    Value = x.Id.ToString(),
                    Selected = model.ClassDivisionId == x.Id
                }).OrderBy(x => x.Text).ToList();

                // Apply Class Room Division Search
                var assessment = _smsService.GetAssessmentById(model.AssessmentId);
                if (assessment == null)
                    throw new ArgumentNullException("id");

                model.Assessment = assessment.Name;
                model.AssessmentId = assessment.Id;
                model.DifficultyLevel = assessment.DifficultyLevelId > 0 ? EnumExtensions.GetDescriptionByValue<DifficultyLevel>(assessment.DifficultyLevelId) : "";
                model.IsTimeBound = assessment.IsTimeBound;
                model.MaxMarks = assessment.MaxMarks;
                model.PassingMarks = assessment.PassingMarks;
                model.StringStartTime = assessment.StartTime.Value.ToString("MMMM dd, yyyy HH:mm tt");
                model.StringEndTime = assessment.EndTime.Value.ToString("MMMM dd, yyyy HH:mm tt");
                model.Subject = assessment.SubjectId.HasValue && assessment.SubjectId.Value > 0 ? _smsService.GetSubjectById(assessment.SubjectId.Value).Name : "-";
                model.Url = assessment.Url;
                model.Duration = assessment.DurationInMinutes;
                model.StartTime = assessment.StartTime.Value;
                model.EndTime = assessment.EndTime.Value;
                var studentsAssociated = _smsService.GetStudentsByAssessmentId(model.AssessmentId);
                // Subjects
                var pSubjects = new List<int>();

                if (assessment.SubjectId.HasValue)
                    pSubjects.Add(assessment.SubjectId.Value);

                var allstudents = _smsService.SearchStudents(classid: model.ClassDivisionId);
                var allHolidays = _smsService.GetAllHolidaysByAcadmicYear(assessment.AcadmicYearId);
                model.Holidays = allHolidays.Select(x => x.Date.Value.ToString("MMMM dd, yyyy")).ToArray();

                model.List.Clear();
                foreach (var q in allstudents)
                {
                    model.List.Add(new AssessmentStudentModel()
                    {
                        AssessmentId = model.AssessmentId,
                        Assessment = assessment.Name,
                        AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
                                                select new SelectListItem
                                                {
                                                    Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
                                                    Value = Convert.ToInt32(d).ToString(),
                                                    Selected = false
                                                }).ToList(),
                        AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
                                                   select new SelectListItem
                                                   {
                                                       Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
                                                       Value = Convert.ToInt32(d).ToString(),
                                                       Selected = false
                                                   }).ToList(),
                        Id = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Id : 0),
                        CertificateHtml = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).CertificateHtml : ""),
                        GradeSystemId = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).GradeSystemId : 0),
                        IsCompleted = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).IsCompleted : false),
                        StudentId = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).StudentId : q.Id),
                        IsActive = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).IsActive : false),
                        IsExpired = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).IsExpired : false),
                        MarksObtained = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).MarksObtained : 0),
                        ResultStatusId = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).ResultStatusId : 0),
                        Url = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Url : ""),
                        UserId = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).UserId : q.UserId),
                        IsChecked = (studentsAssociated.Any(x => x.StudentId == q.Id) ? true : false),
                        StartOn = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).StartOn : assessment.StartTime),
                        EndOn = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).EndOn : assessment.EndTime),
                        Student = (studentsAssociated.Any(x => x.StudentId == q.Id) ? studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Student.FName + (!string.IsNullOrEmpty(studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Student.LName) ? (" " + studentsAssociated.FirstOrDefault(x => x.StudentId == q.Id).Student.LName) : "") : _smsService.GetStudentById(q.Id).FName + (!string.IsNullOrEmpty(_smsService.GetStudentById(q.Id).LName) ? (" " + _smsService.GetStudentById(q.Id).LName) : ""))
                    });
                }

                return View(model);
            }
            else
            {

                if (ModelState.IsValid && model.SelectedStudent != null && model.SelectedStudent.Length > 0)
                {
                    var alreadyAddedStudents = _smsService.GetStudentsByAssessmentId(model.AssessmentId);
                    foreach (var id in model.SelectedStudent)
                    {
                        var aStudent = new AssessmentStudent();
                        aStudent.StudentId = id;
                        aStudent.AssessmentId = model.AssessmentId;
                        aStudent.StartOn = Convert.ToDateTime(frm["StartOn_" + id].ToString());
                        aStudent.EndOn = Convert.ToDateTime(frm["EndOn_" + id].ToString());
                        aStudent.GradeSystemId = (frm["GradeSystemId_" + id] != null && !string.IsNullOrEmpty(frm["GradeSystemId_" + id].ToString())) ? Convert.ToInt32(frm["GradeSystemId_" + id].ToString()) : 0;
                        aStudent.ResultStatusId = (frm["ResultStatusId_" + id] != null && !string.IsNullOrEmpty(frm["ResultStatusId_" + id].ToString())) ? Convert.ToInt32(frm["ResultStatusId_" + id].ToString()) : 0;
                        aStudent.UserId = _userContext.CurrentUser.Id;
                        aStudent.IsCompleted = (frm["IsCompleted_" + id] != null && !string.IsNullOrEmpty(frm["IsCompleted_" + id].ToString())) ? Convert.ToBoolean(frm["IsCompleted_" + id].ToString()) : false;
                        aStudent.IsExpired = (frm["IsExpired_" + id] != null && !string.IsNullOrEmpty(frm["IsExpired_" + id].ToString())) ? Convert.ToBoolean(frm["IsExpired_" + id].ToString()) : false;
                        aStudent.Url = (frm["Url_" + id] != null && !string.IsNullOrEmpty(frm["Url_" + id].ToString())) ? (_urlHelper.GetLocation(false) +  frm["Url_" + id].ToString()) : "";

                        if (alreadyAddedStudents.Any(x => x.StudentId == id))
                        {
                            var assessmentStudent = alreadyAddedStudents.FirstOrDefault(x => x.StudentId == id);
                            assessmentStudent.StartOn = aStudent.StartOn;
                            assessmentStudent.EndOn = aStudent.EndOn;
                            assessmentStudent.GradeSystemId = aStudent.GradeSystemId;
                            assessmentStudent.ResultStatusId = aStudent.ResultStatusId;
                            assessmentStudent.UserId = aStudent.UserId;
                            assessmentStudent.IsCompleted = aStudent.IsCompleted;
                            assessmentStudent.IsExpired = aStudent.IsExpired;
                            assessmentStudent.ModifiedOn = DateTime.Now;
                            _smsService.UpdateStudentAssessment(assessmentStudent);
                        }
                        else
                        {
                            aStudent.CreatedOn = aStudent.ModifiedOn = DateTime.Now;
                            aStudent.UserId = _userContext.CurrentUser.Id;
                            _smsService.InsertStudentAssessment(aStudent);
                        }
                    }

                    if (alreadyAddedStudents.Count > 0)
                    {
                        var removeStudents = alreadyAddedStudents.Where(y => model.SelectedStudent.Any(z => z != y.StudentId)).ToList();
                        foreach (var q in removeStudents)
                        {
                            _smsService.DeleteStudentAssessment(q.StudentId);
                        }
                    }
                }
                SuccessNotification("Assessment updated successfully.");
            }

            return RedirectToAction("AssessmentStudents", new { id = model.AssessmentId });
        }

        #endregion

        #region ssessment URLs
        [MVCDecryptFilter(secret = "AssessmentUrl")]
        public ActionResult GetAssessmentExam(int studentid, int assessmentid)
        {
            return View();
        }
        #endregion
    }
}
