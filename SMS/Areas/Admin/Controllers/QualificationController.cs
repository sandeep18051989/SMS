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
    public class QualificationController : AdminAreaController
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

        public QualificationController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var qualificationData = (from tempqualifications in _smsService.GetAllQualifications() select tempqualifications);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    qualificationData = qualificationData.Where(m => m.Name.Contains(searchValue) || m.Description.Contains(searchValue));
                }

                //total number of rows count     
                var lstQualifications = qualificationData as Qualification[] ?? qualificationData.ToArray();
                recordsTotal = lstQualifications.Count();
                //Paging     
                var data = lstQualifications.Skip(skip).Take(pageSize);

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
            if (!_permissionService.Authorize("ManageQualifications"))
                return AccessDeniedView();

            var model = new QualificationModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageQualifications"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new QualificationModel();
            var objQualification = _smsService.GetQualificationById(id);
            if (objQualification != null)
            {
                model = objQualification.ToModel();
            }
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(QualificationModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageQualifications"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkQualification = _smsService.CheckQualificationExists(model.Name, model.Id);
            if (checkQualification)
                ModelState.AddModelError("Name", "A Qualification with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objQualification = _smsService.GetQualificationById(model.Id);
                if (objQualification != null)
                {
                    model.CreatedOn = objQualification.CreatedOn;
                    objQualification = model.ToEntity(objQualification);
                    objQualification.ModifiedOn = DateTime.Now;
                    _smsService.UpdateQualification(objQualification);
                }
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Qualification updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageQualifications"))
                return AccessDeniedView();

            var model = new QualificationModel();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(QualificationModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageQualifications"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkQualification = _smsService.CheckQualificationExists(model.Name, model.Id);
            if (checkQualification)
                ModelState.AddModelError("Name", "A Qualification with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objQualification = model.ToEntity();
                objQualification.CreatedOn = objQualification.ModifiedOn = DateTime.Now;
                objQualification.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertQualification(objQualification);
                SuccessNotification("Qualification created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objQualification.Id });
                }
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageQualifications"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteQualification(id);

            SuccessNotification("Qualification deleted successfully.");
            return RedirectToAction("List");
        }

    }
}
