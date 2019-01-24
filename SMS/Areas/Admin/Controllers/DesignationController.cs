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
    public class DesignationController : AdminAreaController
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

        public DesignationController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var designationData = (from tempdesignations in _smsService.GetAllDesignations() select tempdesignations);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    designationData = designationData.Where(m => m.Name.Contains(searchValue) || m.Description.Contains(searchValue));
                }

                //total number of rows count     
                var lstDesignations = designationData as Designation[] ?? designationData.ToArray();
                recordsTotal = lstDesignations.Count();
                //Paging     
                var data = lstDesignations.Skip(skip).Take(pageSize);

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
            if (!_permissionService.Authorize("ManageDesignations"))
                return AccessDeniedView();

            var model = new DesignationModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageDesignations"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new DesignationModel();
            var objDesignation = _smsService.GetDesignationById(id);
            if (objDesignation != null)
            {
                model = objDesignation.ToModel();
            }
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(DesignationModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageDesignations"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkDesignation = _smsService.CheckDesignationExists(model.Name, model.Id);
            if (checkDesignation)
                ModelState.AddModelError("Name", "A Designation with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objDesignation = _smsService.GetDesignationById(model.Id);
                if (objDesignation != null)
                {
                    model.CreatedOn = objDesignation.CreatedOn;
                    objDesignation = model.ToEntity(objDesignation);
                    objDesignation.ModifiedOn = DateTime.Now;
                    _smsService.UpdateDesignation(objDesignation);
                }
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Designation updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageDesignations"))
                return AccessDeniedView();

            var model = new DesignationModel();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(DesignationModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageDesignations"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkDesignation = _smsService.CheckDesignationExists(model.Name, model.Id);
            if (checkDesignation)
                ModelState.AddModelError("Name", "A Designation with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objDesignation = model.ToEntity();
                objDesignation.CreatedOn = objDesignation.ModifiedOn = DateTime.Now;
                objDesignation.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertDesignation(objDesignation);
                SuccessNotification("Designation created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objDesignation.Id });
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
            if (!_permissionService.Authorize("ManageDesignations"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteDesignation(id);

            SuccessNotification("Designation deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusDesignation(id);
            ViewBag.Result = "Designation updated Successfully";

            return Json(new { Result = true });
        }

    }
}
