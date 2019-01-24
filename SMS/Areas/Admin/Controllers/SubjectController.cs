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

namespace SMS.Areas.Admin.Controllers
{
    public class SubjectController : AdminAreaController
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

        public SubjectController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var subjectData = (from tempsubjects in _smsService.GetAllSubjects() select tempsubjects);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    subjectData = subjectData.Where(m => m.Name.Contains(searchValue) || m.Code.Contains(searchValue));
                }

                //total number of rows count     
                var lstSubjects = subjectData as Subject[] ?? subjectData.ToArray();
                recordsTotal = lstSubjects.Count();
                //Paging     
                var data = lstSubjects.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => x.ToModel()).OrderBy(x => x.Name).ToList()
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
            if (!_permissionService.Authorize("ManageSubject"))
                return AccessDeniedView();

            var model = new SubjectModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageSubject"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new SubjectModel();
            var objSubject = _smsService.GetSubjectById(id);
            if (objSubject != null)
            {
                model = objSubject.ToModel();
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
        public ActionResult Edit(SubjectModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageSubject"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkSubject = _smsService.CheckSubjectExists(model.Name, model.Id);
            if (checkSubject)
                ModelState.AddModelError("Name", "A Subject with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objSubject = _smsService.GetSubjectById(model.Id);
                if (objSubject != null)
                {
                    model.CreatedOn = objSubject.CreatedOn;
                    objSubject = model.ToEntity(objSubject);
                    objSubject.ModifiedOn = DateTime.Now;
                    _smsService.UpdateSubject(objSubject);
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

            SuccessNotification("Subject updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageSubject"))
                return AccessDeniedView();

            var model = new SubjectModel();
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
        public ActionResult Create(SubjectModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageSubject"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkSubject = _smsService.CheckSubjectExists(model.Name, model.Id);
            if (checkSubject)
                ModelState.AddModelError("Name", "A Subject with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objSubject = model.ToEntity();
                objSubject.CreatedOn = objSubject.ModifiedOn = DateTime.Now;
                objSubject.UserId = _userContext.CurrentUser.Id;
                objSubject.Code = EF.Services.CodeHelper.GenerateRandomSubjectCode();
                objSubject.SubjectUniqueId = Guid.NewGuid();
                _smsService.InsertSubject(objSubject);
                SuccessNotification("Subject created successfully.");

                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objSubject.Id });
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
            if (!_permissionService.Authorize("ManageSubject"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteSubject(id);

            SuccessNotification("Subject deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusSubject(id);
            ViewBag.Result = "Subject updated Successfully";

            return Json(new { Result = true });
        }

    }
}
