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

        #endregion Fileds

        #region Constructor

        public AssessmentController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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

            _roleService.Delete(id);

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
                throw new ArgumentNullException("classroomteacher");

            var model = new List<AssessmentQuestionModel>();
            ViewBag.Title = assessment.Name;

            var questionsAssociated = _smsService.GetQuestionsByAssessmentId(id);
            // Subjects
            var pSubjects = new List<int>();

            if (assessment.SubjectId.HasValue)
                pSubjects.Add(assessment.SubjectId.Value);

            var allquestions = _smsService.SearchQuestions(subjectids: pSubjects.ToArray(), difficultylevel:(assessment.DifficultyLevelId),onlytimebound: assessment.IsTimeBound);
            foreach (var q in allquestions)
            {
                model.Add(new AssessmentQuestionModel() {
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
                    UserId = (questionsAssociated.Any(x => x.QuestionId == q.Id) ? questionsAssociated.FirstOrDefault(x => x.QuestionId == q.Id).UserId : q.UserId)
                });
            }

            model = model.OrderBy(x => x.DisplayOrder).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AssessmentQuestions(IList<AssessmentQuestionModel> model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageAssessment"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                //var objTeacherAssessment = model.ToEntity();
                //objTeacherAssessment.Id = 0;
                //objTeacherAssessment.CreatedOn = objTeacherAssessment.ModifiedOn = DateTime.Now;
                //objTeacherAssessment.UserId = _userContext.CurrentUser.Id;
                //_smsService.InsertTeacherAssessment(objTeacherAssessment);
            }
            //else
            //{
            //    var assessmentsAlreadyAssociated = _smsService.GetAllAssessmentsByTeacher(model.TeacherId);
            //    model.AvailableAssessments = _smsService.GetAllAssessments().Select(x => new SelectListItem()
            //    {
            //        Text = x.AssessmentName.Trim(),
            //        Value = x.Id.ToString(),
            //        Selected = x.Id == model.AssessmentId
            //    }).Where(x => !assessmentsAlreadyAssociated.Any(y => y.AssessmentId.ToString() == x.Value)).ToList();

            //    model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            //    {
            //        Text = x.Name.Trim(),
            //        Value = x.Id.ToString(),
            //        Selected = x.Id == model.AcadmicYearId
            //    }).ToList();

            //    var vacantClassRooms = _smsService.GetVacantClassRoomsForAssessments();
            //    model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
            //    {
            //        Text = x.Number.Trim(),
            //        Value = x.Id.ToString(),
            //        Selected = x.Id == model.ClassRoomId
            //    }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

            //    model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
            //                                  select new SelectListItem
            //                                  {
            //                                      Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
            //                                      Value = Convert.ToInt32(d).ToString(),
            //                                      Selected = model.GradeSystemId == Convert.ToInt32(d)
            //                                  }).ToList();

            //    model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
            //                                     select new SelectListItem
            //                                     {
            //                                         Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
            //                                         Value = Convert.ToInt32(d).ToString(),
            //                                         Selected = model.ResultStatusId == Convert.ToInt32(d)
            //                                     }).ToList();
            //    return View(model);
            //}

            SuccessNotification("Assessment updated successfully.");
            return RedirectToAction("AssessmentQuestions", new { id = model.FirstOrDefault().AssessmentId });
        }


        //public ActionResult EditTeacherAssessment(int id)
        //{
        //    if (!_permissionService.Authorize("ManageAssessment"))
        //        return AccessDeniedView();

        //    if (id == 0)
        //        throw new ArgumentNullException("id");

        //    var assessmentTeacher = _smsService.GetTeacherAssessmentMappingById(id);
        //    if (assessmentTeacher == null)
        //        throw new ArgumentNullException("Assessment Teacher");

        //    var model = assessmentTeacher.ToModel();
        //    var assessmentsAlreadyAssociated = _smsService.GetAllAssessmentsByTeacher(id);

        //    model.AvailableAssessments = _smsService.GetAllAssessments().Select(x => new SelectListItem()
        //    {
        //        Text = x.AssessmentName.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.AssessmentId
        //    }).Where(x => !assessmentsAlreadyAssociated.Any(y => (y.AssessmentId.ToString() == x.Value)) || model.AssessmentId.ToString() == x.Value).ToList();

        //    model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
        //    {
        //        Text = x.Name.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.AcadmicYearId
        //    }).ToList();

        //    var vacantClassRooms = _smsService.GetVacantClassRoomsForAssessments();
        //    model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
        //    {
        //        Text = x.Number.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.ClassRoomId
        //    }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

        //    model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
        //                                  select new SelectListItem
        //                                  {
        //                                      Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
        //                                      Value = Convert.ToInt32(d).ToString(),
        //                                      Selected = false
        //                                  }).ToList();

        //    model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
        //                                     select new SelectListItem
        //                                     {
        //                                         Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
        //                                         Value = Convert.ToInt32(d).ToString(),
        //                                         Selected = false
        //                                     }).ToList();
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult EditTeacherAssessment(TeacherAssessmentModel model)
        //{
        //    if (!_permissionService.Authorize("ManageAssessment"))
        //        return AccessDeniedView();

        //    if (ModelState.IsValid)
        //    {
        //        model.UserId = _userContext.CurrentUser.Id;
        //        var teacherAssessment = _smsService.GetTeacherAssessmentMappingById(model.Id);
        //        teacherAssessment = model.ToEntity(teacherAssessment);
        //        teacherAssessment.CreatedOn = teacherAssessment.ModifiedOn = DateTime.Now;
        //        _smsService.UpdateTeacherAssessment(teacherAssessment);
        //    }
        //    else
        //    {
        //        var assessmentsAlreadyAssociated = _smsService.GetAllAssessmentsByTeacher(model.TeacherId);
        //        model.AvailableAssessments = _smsService.GetAllAssessments().Select(x => new SelectListItem()
        //        {
        //            Text = x.AssessmentName.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.AssessmentId
        //        }).Where(x => !assessmentsAlreadyAssociated.Any(y => (y.AssessmentId.ToString() == x.Value)) || model.AssessmentId.ToString() == x.Value).ToList();

        //        model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
        //        {
        //            Text = x.Name.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.AcadmicYearId
        //        }).ToList();

        //        var vacantClassRooms = _smsService.GetVacantClassRoomsForAssessments();
        //        model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
        //        {
        //            Text = x.Number.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.ClassRoomId
        //        }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

        //        model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
        //                                      select new SelectListItem
        //                                      {
        //                                          Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
        //                                          Value = Convert.ToInt32(d).ToString(),
        //                                          Selected = false
        //                                      }).ToList();

        //        model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
        //                                         select new SelectListItem
        //                                         {
        //                                             Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
        //                                             Value = Convert.ToInt32(d).ToString(),
        //                                             Selected = false
        //                                         }).ToList();
        //        return View(model);
        //    }

        //    SuccessNotification("Teacher Assessment Updated Successfully.");
        //    return RedirectToAction("EditTeacherAssessment", new { id = model.Id });
        //}

        #endregion

        #region Student Assessment

        //public ActionResult StudentAssessments(int id)
        //{
        //    if (!_permissionService.Authorize("ManageAssessment"))
        //        return AccessDeniedView();

        //    if (id == 0)
        //        throw new ArgumentNullException("id");

        //    var classStudent = _smsService.GetStudentById(id);
        //    if (classStudent == null)
        //        throw new ArgumentNullException("student");

        //    var model = new StudentAssessmentModel();
        //    model.StudentId = classStudent.Id;
        //    model.Student = classStudent.FName + (!string.IsNullOrEmpty(classStudent.MName) ? (" " + classStudent.MName) : "") + (!string.IsNullOrEmpty(classStudent.LName) ? (" " + classStudent.LName) : "");
        //    model.UserId = _userContext.CurrentUser.Id;

        //    var assessmentsAlreadyAssociated = _smsService.GetAllAssessmentsByStudent(id);
        //    model.AvailableAssessments = _smsService.GetAllAssessments().Select(x => new SelectListItem()
        //    {
        //        Text = x.AssessmentName.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.AssessmentId
        //    }).Where(x => !assessmentsAlreadyAssociated.Any(y => y.AssessmentId.ToString() == x.Value)).ToList();

        //    model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
        //    {
        //        Text = x.Name.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.AcadmicYearId
        //    }).ToList();

        //    var vacantClassRooms = _smsService.GetVacantClassRoomsForAssessments();
        //    model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
        //    {
        //        Text = x.Number.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.ClassRoomId
        //    }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

        //    model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
        //                                  select new SelectListItem
        //                                  {
        //                                      Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
        //                                      Value = Convert.ToInt32(d).ToString(),
        //                                      Selected = false
        //                                  }).ToList();

        //    model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
        //                                     select new SelectListItem
        //                                     {
        //                                         Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
        //                                         Value = Convert.ToInt32(d).ToString(),
        //                                         Selected = false
        //                                     }).ToList();
        //    return View(model);
        //}

        //[HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult StudentAssessments(StudentAssessmentModel model, FormCollection frm)
        //{
        //    if (!_permissionService.Authorize("ManageAssessment"))
        //        return AccessDeniedView();

        //    if (ModelState.IsValid)
        //    {
        //        var objStudentAssessment = model.ToEntity();
        //        objStudentAssessment.Id = 0;
        //        objStudentAssessment.CreatedOn = objStudentAssessment.ModifiedOn = DateTime.Now;
        //        objStudentAssessment.UserId = _userContext.CurrentUser.Id;
        //        _smsService.InsertStudentAssessment(objStudentAssessment);
        //    }
        //    else
        //    {
        //        var assessmentsAlreadyAssociated = _smsService.GetAllAssessmentsByStudent(model.StudentId);
        //        model.AvailableAssessments = _smsService.GetAllAssessments().Select(x => new SelectListItem()
        //        {
        //            Text = x.AssessmentName.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.AssessmentId
        //        }).Where(x => !assessmentsAlreadyAssociated.Any(y => y.AssessmentId.ToString() == x.Value)).ToList();

        //        model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
        //        {
        //            Text = x.Name.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.AcadmicYearId
        //        }).ToList();

        //        var vacantClassRooms = _smsService.GetVacantClassRoomsForAssessments();
        //        model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
        //        {
        //            Text = x.Number.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.ClassRoomId
        //        }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

        //        model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
        //                                      select new SelectListItem
        //                                      {
        //                                          Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
        //                                          Value = Convert.ToInt32(d).ToString(),
        //                                          Selected = model.GradeSystemId == Convert.ToInt32(d)
        //                                      }).ToList();

        //        model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
        //                                         select new SelectListItem
        //                                         {
        //                                             Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
        //                                             Value = Convert.ToInt32(d).ToString(),
        //                                             Selected = model.ResultStatusId == Convert.ToInt32(d)
        //                                         }).ToList();
        //        return View(model);
        //    }

        //    SuccessNotification("Assessment updated successfully.");
        //    return RedirectToAction("StudentAssessments", new { id = model.Id });
        //}

        //public ActionResult EditStudentAssessment(int id)
        //{
        //    if (!_permissionService.Authorize("ManageAssessment"))
        //        return AccessDeniedView();

        //    if (id == 0)
        //        throw new ArgumentNullException("id");

        //    var assessmentStudent = _smsService.GetStudentAssessmentMappingById(id);
        //    if (assessmentStudent == null)
        //        throw new ArgumentNullException("Assessment Student");

        //    var model = assessmentStudent.ToModel();
        //    var assessmentsAlreadyAssociated = _smsService.GetAllAssessmentsByStudent(id);

        //    model.AvailableAssessments = _smsService.GetAllAssessments().Select(x => new SelectListItem()
        //    {
        //        Text = x.AssessmentName.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.AssessmentId
        //    }).Where(x => !assessmentsAlreadyAssociated.Any(y => (y.AssessmentId.ToString() == x.Value)) || model.AssessmentId.ToString() == x.Value).ToList();

        //    model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
        //    {
        //        Text = x.Name.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.AcadmicYearId
        //    }).ToList();

        //    var vacantClassRooms = _smsService.GetVacantClassRoomsForAssessments();
        //    model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
        //    {
        //        Text = x.Number.Trim(),
        //        Value = x.Id.ToString(),
        //        Selected = x.Id == model.ClassRoomId
        //    }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

        //    model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
        //                                  select new SelectListItem
        //                                  {
        //                                      Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
        //                                      Value = Convert.ToInt32(d).ToString(),
        //                                      Selected = false
        //                                  }).ToList();

        //    model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
        //                                     select new SelectListItem
        //                                     {
        //                                         Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
        //                                         Value = Convert.ToInt32(d).ToString(),
        //                                         Selected = false
        //                                     }).ToList();
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult EditStudentAssessment(StudentAssessmentModel model)
        //{
        //    if (!_permissionService.Authorize("ManageAssessment"))
        //        return AccessDeniedView();

        //    if (ModelState.IsValid)
        //    {
        //        model.UserId = _userContext.CurrentUser.Id;
        //        var studentAssessment = _smsService.GetStudentAssessmentMappingById(model.Id);
        //        studentAssessment = model.ToEntity(studentAssessment);
        //        studentAssessment.CreatedOn = studentAssessment.ModifiedOn = DateTime.Now;
        //        _smsService.UpdateStudentAssessment(studentAssessment);
        //    }
        //    else
        //    {
        //        var assessmentsAlreadyAssociated = _smsService.GetAllAssessmentsByStudent(model.StudentId);
        //        model.AvailableAssessments = _smsService.GetAllAssessments().Select(x => new SelectListItem()
        //        {
        //            Text = x.AssessmentName.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.AssessmentId
        //        }).Where(x => !assessmentsAlreadyAssociated.Any(y => (y.AssessmentId.ToString() == x.Value)) || model.AssessmentId.ToString() == x.Value).ToList();

        //        model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
        //        {
        //            Text = x.Name.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.AcadmicYearId
        //        }).ToList();

        //        var vacantClassRooms = _smsService.GetVacantClassRoomsForAssessments();
        //        model.AvailableClassRooms = _smsService.GetAllClassRooms().Select(x => new SelectListItem()
        //        {
        //            Text = x.Number.Trim(),
        //            Value = x.Id.ToString(),
        //            Selected = x.Id == model.ClassRoomId
        //        }).Where(x => !vacantClassRooms.Any(y => (y.Id.ToString() == x.Value || model.ClassRoomId == y.Id))).ToList();

        //        model.AvailableGradeSystem = (from GradeSystem d in Enum.GetValues(typeof(GradeSystem))
        //                                      select new SelectListItem
        //                                      {
        //                                          Text = EnumExtensions.GetDescriptionByValue<GradeSystem>(Convert.ToInt32(d)),
        //                                          Value = Convert.ToInt32(d).ToString(),
        //                                          Selected = false
        //                                      }).ToList();

        //        model.AvailableResultStatuses = (from ResultStatus d in Enum.GetValues(typeof(ResultStatus))
        //                                         select new SelectListItem
        //                                         {
        //                                             Text = EnumExtensions.GetDescriptionByValue<ResultStatus>(Convert.ToInt32(d)),
        //                                             Value = Convert.ToInt32(d).ToString(),
        //                                             Selected = false
        //                                         }).ToList();
        //        return View(model);
        //    }

        //    SuccessNotification("Student Assessment Updated Successfully.");
        //    return RedirectToAction("EditStudentAssessment", new { id = model.Id });
        //}

        #endregion
    }
}
