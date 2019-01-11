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
    public class AcadmicYearController : AdminAreaController
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

        public AcadmicYearController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var acadmicyearData = (from tempacadmicyeares in _smsService.GetAllAcadmicYears() select tempacadmicyeares);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    acadmicyearData = acadmicyearData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstAcadmicYeares = acadmicyearData as AcadmicYear[] ?? acadmicyearData.ToArray();
                recordsTotal = lstAcadmicYeares.Count();
                //Paging     
                var data = lstAcadmicYeares.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new AcadmicYearModel()
                        {
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            Id = x.Id
                        }).OrderByDescending(x => x.IsActive).ToList()
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
            if (!_permissionService.Authorize("ManageAcadmicYear"))
                return AccessDeniedView();

            var model = new AcadmicYearModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageAcadmicYear"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new AcadmicYearModel();
            var objAcadmicYear = _smsService.GetAcadmicYearById(id);
            if (objAcadmicYear != null)
            {
                model = objAcadmicYear.ToModel();
            }
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(AcadmicYearModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageAcadmicYear"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate acadmicyear, if any
            var checkAcadmicYear = _smsService.CheckAcadmicYearExists(model.Name, model.Id);
            if (checkAcadmicYear)
                ModelState.AddModelError("Name", "AcadmicYear with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objAcadmicYear = _smsService.GetAcadmicYearById(model.Id);
                if (objAcadmicYear != null)
                {
                    model.CreatedOn = objAcadmicYear.CreatedOn;
                    objAcadmicYear = model.ToEntity(objAcadmicYear);
                    objAcadmicYear.ModifiedOn = DateTime.Now;

                    if (model.IsActive)
                        _smsService.DeactivateAllAcadmicYears();

                    _smsService.UpdateAcadmicYear(objAcadmicYear);
                }
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Acadmic Year updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageAcadmicYear"))
                return AccessDeniedView();

            var model = new AcadmicYearModel();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(AcadmicYearModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageAcadmicYear"))
                return AccessDeniedView();

            // Check for duplicate acadmicyear, if any
            var checkAcadmicYear = _smsService.CheckAcadmicYearExists(model.Name, model.Id);
            if (checkAcadmicYear)
                ModelState.AddModelError("Name", "Acadmic Year with the same name already exists.");

            if (ModelState.IsValid)
            {
                var objAcadmicYear = model.ToEntity();
                objAcadmicYear.CreatedOn = objAcadmicYear.ModifiedOn = DateTime.Now;
                objAcadmicYear.UserId = _userContext.CurrentUser.Id;

                if (model.IsActive)
                    _smsService.DeactivateAllAcadmicYears();

                _smsService.InsertAcadmicYear(objAcadmicYear);
                SuccessNotification("Acadmic Year created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objAcadmicYear.Id });
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
            if (!_permissionService.Authorize("ManageAcadmicYear"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _roleService.Delete(id);

            SuccessNotification("Acadmic Year deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusAcadmicYear(id);
            ViewBag.Result = "Acadmic Year updated Successfully";

            return Json(new { Result = true });
        }

    }
}
