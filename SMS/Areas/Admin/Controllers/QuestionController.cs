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
    public class QuestionController : AdminAreaController
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

        public QuestionController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var questionData = (from tempquestion in _smsService.GetAllQuestions() select tempquestion);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    questionData = questionData.Where(m => m.Name.Contains(searchValue) || m.Explanation.Contains(searchValue));
                }

                //total number of rows count     
                var lstQuestion = questionData as Question[] ?? questionData.ToArray();
                recordsTotal = lstQuestion.Count();
                //Paging     
                var data = lstQuestion.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new QuestionModel() {
                            Id = x.Id,
                            Name = x.Name,
                            SubjectId = x.SubjectId,
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            DifficultyLevelId = x.DifficultyLevelId,
                            Explanation = x.Explanation,
                            IsTimeBound = x.IsTimeBound,
                            NegativeMarks = x.NegativeMarks,
                            QuestionGuid = x.QuestionGuid,
                            QuestionTypeId = x.QuestionTypeId,
                            RightMarks = x.RightMarks,
                            SolveTime = x.SolveTime,
                            Difficulty = x.DifficultyLevelId > 0 ? Enum.GetValues(typeof(DifficultyLevel)).GetValue(x.DifficultyLevelId).ToString() : "",
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
        [HttpPost]
        public ActionResult LoadOptionGrid(int id)
        {
            try
            {
                var optionData = (from associatedoption in _smsService.GetOptionsByQuestionId(id) select associatedoption).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = optionData.Select(x => new OptionModel()
                        {
                            Id = x.Id,
                            CorrectAnswer = x.CorrectAnswer,
                            DisplayOrder = x.DisplayOrder,
                            Name = x.Name,
                            QuestionId = x.QuestionId,
                            UserId = x.UserId
                        }).OrderBy(x => x.DisplayOrder).ToList()
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
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            var model = new QuestionModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new QuestionModel();
            var objQuestion = _smsService.GetQuestionById(id);
            if (objQuestion != null)
            {
                model = objQuestion.ToModel();
            }

            model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.SubjectId > 0 ? model.SubjectId == x.Id : false
            }).ToList();

            model.AvailableQuestionTypes = _smsService.GetAllQuestionTypes().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.QuestionTypeId > 0 && model.QuestionTypeId == x.Id
            }).OrderBy(x => x.Text).ToList();

            model.AvailableLevels = (from DifficultyLevel d in Enum.GetValues(typeof(DifficultyLevel))
                                           select new SelectListItem
                                           {
                                               Text = d.ToString(),
                                               Value = Convert.ToInt32(d).ToString(),
                                               Selected = (Convert.ToInt32(d) == model.DifficultyLevelId)
                                           }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(QuestionModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkQuestion = _smsService.CheckQuestionExists(model.Name, model.Id);
            if (checkQuestion)
                ModelState.AddModelError("Name", "Question already exists!");

            if (ModelState.IsValid)
            {
                var objQuestion = _smsService.GetQuestionById(model.Id);
                if (objQuestion != null)
                {
                    model.CreatedOn = objQuestion.CreatedOn;
                    objQuestion = model.ToEntity(objQuestion);
                    objQuestion.ModifiedOn = DateTime.Now;
                    _smsService.UpdateQuestion(objQuestion);
                }
            }
            else
            {
                model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.SubjectId > 0 ? model.SubjectId == x.Id : false
                }).ToList();

                model.AvailableQuestionTypes = _smsService.GetAllQuestionTypes().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.QuestionTypeId > 0 && model.QuestionTypeId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableLevels = (from DifficultyLevel d in Enum.GetValues(typeof(DifficultyLevel))
                                         select new SelectListItem
                                         {
                                             Text = d.ToString(),
                                             Value = Convert.ToInt32(d).ToString(),
                                             Selected = (Convert.ToInt32(d) == model.DifficultyLevelId)
                                         }).ToList();
                return View(model);
            }

            SuccessNotification("Question updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            var model = new QuestionModel();
            model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
            }).ToList();

            model.AvailableQuestionTypes = _smsService.GetAllQuestionTypes().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
            }).OrderBy(x => x.Text).ToList();

            model.AvailableLevels = (from DifficultyLevel d in Enum.GetValues(typeof(DifficultyLevel))
                                     select new SelectListItem
                                     {
                                         Text = d.ToString(),
                                         Value = Convert.ToInt32(d).ToString(),
                                     }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(QuestionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkQuestion = _smsService.CheckQuestionExists(model.Name, model.Id);
            if (checkQuestion)
                ModelState.AddModelError("Name", "Question already exists!");

            if (ModelState.IsValid)
            {
                var objQuestion = model.ToEntity();
                objQuestion.CreatedOn = objQuestion.ModifiedOn = DateTime.Now;
                objQuestion.UserId = _userContext.CurrentUser.Id;
                objQuestion.QuestionGuid = Guid.NewGuid();
                _smsService.InsertQuestion(objQuestion);
                SuccessNotification("Question created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objQuestion.Id });
                }
            }
            else
            {
                model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.SubjectId > 0 ? model.SubjectId == x.Id : false
                }).ToList();

                model.AvailableQuestionTypes = _smsService.GetAllQuestionTypes().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.QuestionTypeId > 0 && model.QuestionTypeId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableLevels = (from DifficultyLevel d in Enum.GetValues(typeof(DifficultyLevel))
                                         select new SelectListItem
                                         {
                                             Text = d.ToString(),
                                             Value = Convert.ToInt32(d).ToString(),
                                             Selected = (Convert.ToInt32(d) == model.DifficultyLevelId)
                                         }).ToList();
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _roleService.Delete(id);

            SuccessNotification("Question deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusQuestion(id);
            ViewBag.Result = "Question updated Successfully";

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult RemoveOptionFromQuestion(int id, int optionid)
        {
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Question id not found");

            var objQuestion = _smsService.GetQuestionById(id);
            if (objQuestion != null)
            {
                var selectOption = _smsService.GetOptionById(id);
                if (selectOption != null)
                {
                    _smsService.DeleteOption(optionid);
                }
            }

            SuccessNotification("Option removed successfully from selected question.");
            return new JsonResult()
            {
                Data = true,
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

    }
}
