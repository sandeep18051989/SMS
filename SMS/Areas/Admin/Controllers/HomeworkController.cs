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
    public class HomeworkController : AdminAreaController
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

        public HomeworkController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var homeworkData = (from temphomework in _smsService.GetAllHomeworks() select temphomework);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    homeworkData = homeworkData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstHomework = homeworkData as Homework[] ?? homeworkData.ToArray();
                recordsTotal = lstHomework.Count();
                //Paging     
                var data = lstHomework.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new HomeworkModel() {
                            AcadmicYearId = x.AcadmicYearId,
                            Id = x.Id,
                            Name = x.Name,
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            Description = x.Description,
                            CreatedOnString = x.CreatedOn.ToString("U")
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

        #endregion
        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageHomework"))
                return AccessDeniedView();

            var model = new HomeworkModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageHomework"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new HomeworkModel();
            var objHomework = _smsService.GetHomeworkById(id);
            if (objHomework != null)
            {
                model = objHomework.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.AcadmicYearId == x.Id
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(HomeworkModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageHomework"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkHomework = _smsService.CheckHomeworkExists(model.Name, model.Id);
            if (checkHomework)
                ModelState.AddModelError("Name", "A Homework with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objHomework = _smsService.GetHomeworkById(model.Id);
                if (objHomework != null)
                {
                    model.CreatedOn = objHomework.CreatedOn;
                    objHomework = model.ToEntity(objHomework);
                    objHomework.ModifiedOn = DateTime.Now;
                    _smsService.UpdateHomework(objHomework);
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
                return View(model);
            }

            SuccessNotification("Homework updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageHomework"))
                return AccessDeniedView();

            var model = new HomeworkModel();
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
        public ActionResult Create(HomeworkModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageHomework"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkHomework = _smsService.CheckHomeworkExists(model.Name, model.Id);
            if (checkHomework)
                ModelState.AddModelError("Name", "A Homework with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objHomework = model.ToEntity();
                objHomework.CreatedOn = objHomework.ModifiedOn = DateTime.Now;
                objHomework.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertHomework(objHomework);
                SuccessNotification("Homework created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objHomework.Id });
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
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageHomework"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteHomework(id);

            SuccessNotification("Homework deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusHomework(id);
            ViewBag.Result = "Homework updated Successfully";

            return Json(new { Result = true });
        }

    }
}
