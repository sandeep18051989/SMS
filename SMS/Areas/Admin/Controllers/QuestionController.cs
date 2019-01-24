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
                        data = data.Select(x => new QuestionModel()
                        {
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
                            Difficulty = x.DifficultyLevelId > 0 ? EnumExtensions.GetDescriptionByValue<DifficultyLevel>(x.DifficultyLevelId) : "",
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
            model.OptionCount = _smsService.GetOptionsByQuestionId(model.Id).Count;

            if(model.QuestionTypeId == 4)
            {
                model.MatchFollowingOptions = _smsService.GetOptionsByQuestionId(model.Id).Select(x => x.ToModel()).OrderBy(x => x.DisplayOrder).ToList();
            }

            model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim() + "(" + x.Code + ")",
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
                                         Text = EnumExtensions.GetDescriptionByValue<DifficultyLevel>(Convert.ToInt32(d)),
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
                // Get Options
                var options = new List<OptionModel>();
                var frmOptions = new List<string>();
                switch (model.QuestionTypeId)
                {
                    case 1:
                        {
                            frmOptions = frm.AllKeys.Where(x => x.StartsWith("multiplechoice_")).ToList();
                            if (frmOptions.Count > 0)
                            {
                                int count = _smsService.GetOptionsByQuestionId(model.Id).Count;
                                int optionId = 0;
                                int questionId = 0;
                                foreach (var key in frmOptions)
                                {
                                    count += 1;
                                    questionId = Convert.ToInt32(key.Split('_')[1]);
                                    optionId = Convert.ToInt32(key.Split('_')[2]);
                                    if (questionId == 0)
                                    {
                                        var newOption = new Option();
                                        if (frm[key] != null && !string.IsNullOrEmpty(frm[key].ToString()))
                                        {
                                            newOption.Name = frm[key].ToString();
                                            newOption.DisplayOrder = count;
                                            newOption.CreatedOn = newOption.ModifiedOn = DateTime.Now;
                                            newOption.QuestionId = model.Id;
                                            newOption.UserId = _userContext.CurrentUser.Id;

                                            var frmCorrectAnswer = frm.AllKeys.Where(x => x.StartsWith("correctanswer_" + questionId + "_" + optionId)).FirstOrDefault();
                                            if (frmCorrectAnswer != null && !string.IsNullOrEmpty(frm[frmCorrectAnswer].ToString()))
                                            {
                                                newOption.CorrectAnswer = frm[frmCorrectAnswer].ToString();
                                            }
                                            else
                                            {
                                                newOption.CorrectAnswer = "";
                                            }
                                            _smsService.InsertOption(newOption);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            frmOptions = frm.AllKeys.Where(x => x.StartsWith("multipleresponse_")).ToList();
                            if (frmOptions.Count > 0)
                            {
                                int count = _smsService.GetOptionsByQuestionId(model.Id).Count;
                                int optionId = 0;
                                int questionId = 0;
                                foreach (var key in frmOptions)
                                {
                                    count += 1;
                                    questionId = Convert.ToInt32(key.Split('_')[1]);
                                    optionId = Convert.ToInt32(key.Split('_')[2]);
                                    if (questionId == 0)
                                    {
                                        var newOption = new Option();
                                        if (frm[key] != null && !string.IsNullOrEmpty(frm[key].ToString()))
                                        {
                                            newOption.Name = frm[key].ToString();
                                            newOption.DisplayOrder = count;
                                            newOption.CreatedOn = newOption.ModifiedOn = DateTime.Now;
                                            newOption.QuestionId = model.Id;
                                            newOption.UserId = _userContext.CurrentUser.Id;

                                            var frmCorrectAnswer = frm.AllKeys.Where(x => x.StartsWith("correctanswer_" + questionId + "_" + optionId)).FirstOrDefault();
                                            if (frmCorrectAnswer != null && !string.IsNullOrEmpty(frm[frmCorrectAnswer].ToString()))
                                            {
                                                newOption.CorrectAnswer = frm[frmCorrectAnswer].ToString();
                                            }
                                            else
                                            {
                                                newOption.CorrectAnswer = "";
                                            }
                                            _smsService.InsertOption(newOption);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            frmOptions = frm.AllKeys.Where(x => x.StartsWith("fillintheblank_")).ToList();
                            if (frmOptions.Count > 0)
                            {
                                int count = _smsService.GetOptionsByQuestionId(model.Id).Count;
                                int optionId = 0;
                                int questionId = 0;
                                foreach (var key in frmOptions)
                                {
                                    count += 1;
                                    questionId = Convert.ToInt32(key.Split('_')[1]);
                                    optionId = Convert.ToInt32(key.Split('_')[2]);
                                    if (questionId == 0)
                                    {
                                        var newOption = new Option();
                                        if (frm[key] != null && !string.IsNullOrEmpty(frm[key].ToString()))
                                        {
                                            newOption.Name = frm[key].ToString();
                                            newOption.DisplayOrder = count;
                                            newOption.CreatedOn = newOption.ModifiedOn = DateTime.Now;
                                            newOption.QuestionId = model.Id;
                                            newOption.UserId = _userContext.CurrentUser.Id;

                                            var frmCorrectAnswer = frm.AllKeys.Where(x => x.StartsWith("correctanswer_" + questionId + "_" + optionId)).FirstOrDefault();
                                            if (frmCorrectAnswer != null && !string.IsNullOrEmpty(frm[frmCorrectAnswer].ToString()))
                                            {
                                                newOption.CorrectAnswer = frm[frmCorrectAnswer].ToString();
                                            }
                                            else
                                            {
                                                newOption.CorrectAnswer = "";
                                            }
                                            _smsService.InsertOption(newOption);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 4:
                        {
                            var leftOptions = frm.AllKeys.Where(x => x.StartsWith("left_")).ToList();
                            var rightOptions = frm.AllKeys.Where(x => x.StartsWith("right_")).ToList();
                            string correctAnswer = "";
                            var frmCorrectAnswer = frm.AllKeys.Where(x => x.StartsWith("correctanswer")).FirstOrDefault();
                            if (frmCorrectAnswer != null && !string.IsNullOrEmpty(frm[frmCorrectAnswer].ToString()))
                            {
                                correctAnswer = frm[frmCorrectAnswer].ToString();
                            }

                            if (leftOptions.Count > 0 && rightOptions.Count > 0)
                            {
                                // Update Already Added Options
                                var alreadyAddedOptions = _smsService.GetOptionsByQuestionId(model.Id);
                                foreach (var option in alreadyAddedOptions)
                                {
                                    option.CorrectAnswer = correctAnswer;
                                    _smsService.UpdateOption(option);
                                }

                                int count = alreadyAddedOptions.Count;
                                int optionId = 0;
                                int questionId = 0;
                                foreach (var key in leftOptions)
                                {
                                    count += 1;
                                    questionId = Convert.ToInt32(key.Split('_')[1]);
                                    optionId = Convert.ToInt32(key.Split('_')[2]);
                                    if (questionId == 0)
                                    {
                                        var newOption = new Option();
                                        if ((frm[key] != null && !string.IsNullOrEmpty(frm[key].ToString())) && (frm["right_" + questionId + "_" + optionId] != null && !string.IsNullOrEmpty(frm["right_" + questionId + "_" + optionId].ToString())))
                                        {
                                            newOption.Name = frm[key].ToString() + "," + frm["right_" + questionId + "_" + optionId].ToString();
                                            newOption.DisplayOrder = count;
                                            newOption.CreatedOn = newOption.ModifiedOn = DateTime.Now;
                                            newOption.QuestionId = model.Id;
                                            newOption.UserId = _userContext.CurrentUser.Id;
                                            newOption.CorrectAnswer = correctAnswer;
                                            _smsService.InsertOption(newOption);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 5:
                        {
                            break;
                        }
                    case 6:
                        {
                            frmOptions = frm.AllKeys.Where(x => x.StartsWith("chooseanswer_")).ToList();
                            if (frmOptions.Count > 0)
                            {
                                int count = _smsService.GetOptionsByQuestionId(model.Id).Count;
                                int optionId = 0;
                                int questionId = 0;
                                foreach (var key in frmOptions)
                                {
                                    count += 1;
                                    questionId = Convert.ToInt32(key.Split('_')[1]);
                                    optionId = Convert.ToInt32(key.Split('_')[2]);
                                    if (questionId == 0)
                                    {
                                        var newOption = new Option();
                                        if (frm[key] != null && !string.IsNullOrEmpty(frm[key].ToString()))
                                        {
                                            newOption.Name = frm[key].ToString();
                                            newOption.DisplayOrder = count;
                                            newOption.CreatedOn = newOption.ModifiedOn = DateTime.Now;
                                            newOption.QuestionId = model.Id;
                                            newOption.UserId = _userContext.CurrentUser.Id;

                                            var frmCorrectAnswer = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "correctanswer");
                                            if (frmCorrectAnswer != null && !string.IsNullOrEmpty(frm[frmCorrectAnswer].ToString()))
                                            {
                                                newOption.CorrectAnswer = frm[frmCorrectAnswer].ToString();
                                            }
                                            else
                                            {
                                                newOption.CorrectAnswer = "";
                                            }
                                            _smsService.InsertOption(newOption);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                var objQuestion = _smsService.GetQuestionById(model.Id);
                if (objQuestion != null)
                {
                    model.CreatedOn = objQuestion.CreatedOn;
                    objQuestion = model.ToEntity(objQuestion);
                    objQuestion.ModifiedOn = DateTime.Now;
                    _smsService.UpdateQuestion(objQuestion);
                    SuccessNotification("Question updated successfully.");
                    if (continueEditing)
                    {
                        return RedirectToAction("Edit", new { id = model.Id });
                    }
                }
            }
            else
            {
                model.AvailableSubjects = _smsService.GetAllSubjects().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim() + "(" + x.Code + ")",
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
                                             Text = EnumExtensions.GetDescriptionByValue<DifficultyLevel>(Convert.ToInt32(d)),
                                             Value = Convert.ToInt32(d).ToString(),
                                             Selected = (Convert.ToInt32(d) == model.DifficultyLevelId)
                                         }).ToList();
                return View(model);
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
                Text = x.Name.Trim() + "(" + x.Code + ")",
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
                                         Text = EnumExtensions.GetDescriptionByValue<DifficultyLevel>(Convert.ToInt32(d)),
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
                    Text = x.Name.Trim() + "(" + x.Code + ")",
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
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteQuestion(id);

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
        public ActionResult RemoveOptionFromQuestion(int id)
        {
            if (!_permissionService.Authorize("ManageQuestion"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Question id not found");

            var selectOption = _smsService.GetOptionById(id);
            if (selectOption != null)
            {
                var question = _smsService.GetQuestionById(selectOption.QuestionId);

                // Delete Option
                _smsService.DeleteOption(id);

                if (question != null && question.QuestionTypeId == 4)
                {
                    var allOptions = _smsService.GetOptionsByQuestionId(question.Id);
                    if (allOptions.Count > 0)
                    {
                        var newAnswer = new List<string>();
                        foreach (var option in allOptions)
                        {
                            var correctSequence = option.CorrectAnswer.Split(',');
                            newAnswer.Clear();
                            for (int i = 0; i < correctSequence.Length; i++)
                            {
                                if(allOptions.Any(x => x.DisplayOrder == Convert.ToInt32(correctSequence[i])))
                                {
                                    newAnswer.Add(correctSequence[i]);
                                }
                            }

                            option.CorrectAnswer = string.Join(",", newAnswer.ToArray());
                            _smsService.UpdateOption(option);
                        }
                    }
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
