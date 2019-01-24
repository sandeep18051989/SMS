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
    public class AllowanceController : AdminAreaController
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

        public AllowanceController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var allowanceData = (from tempallowance in _smsService.GetAllAllowances() select tempallowance);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allowanceData = allowanceData.Where(m => m.Designation.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstAllowance = allowanceData as Allowance[] ?? allowanceData.ToArray();
                recordsTotal = lstAllowance.Count();
                //Paging     
                var data = lstAllowance.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new AllowanceModel() {
                            AcadmicYearId = x.AcadmicYearId,
                            Id = x.Id,
                            UserId = x.UserId,
                            DA = x.DA,
                            DesignationId=x.DesignationId,
                            Designation = x.DesignationId > 0 ? _smsService.GetDesignationById(x.DesignationId).Name : "",
                            BasicPay = x.BasicPay,
                            ESI = x.ESI,
                            HRA = x.HRA,
                            PF = x.PF,
                            TA = x.TA,
                            TDS = x.TDS,
                            TotalSalary = (x.BasicPay + x.DA + x.TA + x.ESI + x.HRA + x.PF + x.TDS)
                        }).OrderBy(x => x.Designation).ToList()
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
            if (!_permissionService.Authorize("ManageAllowance"))
                return AccessDeniedView();

            var model = new AllowanceModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageAllowance"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new AllowanceModel();
            var objAllowance = _smsService.GetAllowanceById(id);
            if (objAllowance != null)
            {
                model = objAllowance.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.AcadmicYearId == x.Id
            }).ToList();

            model.AvailableDesignations = _smsService.GetAllDesignations().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.DesignationId == x.Id
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(AllowanceModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageAllowance"))
                return AccessDeniedView();

            if (model.DesignationId == 0)
            {
                ModelState.AddModelError("DesignationId", "Please select designation");
            }

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkAllowance = _smsService.CheckAllowanceExistsForDesignation(model.DesignationId, model.Id);
            if (checkAllowance)
                ModelState.AddModelError("DesignationId", "A Allowance with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objAllowance = _smsService.GetAllowanceById(model.Id);
                if (objAllowance != null)
                {
                    model.CreatedOn = objAllowance.CreatedOn;
                    objAllowance = model.ToEntity(objAllowance);
                    objAllowance.ModifiedOn = DateTime.Now;
                    _smsService.UpdateAllowance(objAllowance);
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
                model.AvailableDesignations = _smsService.GetAllDesignations().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.DesignationId == x.Id
                }).ToList();
                return View(model);
            }

            SuccessNotification("Allowance updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageAllowance"))
                return AccessDeniedView();

            var model = new AllowanceModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();
            model.AvailableDesignations = _smsService.GetAllDesignations().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString()
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(AllowanceModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageAllowance"))
                return AccessDeniedView();

            if (model.DesignationId == 0)
                ModelState.AddModelError("DesignationId", "Please select designation");

            // Check for duplicate classroom, if any
            var checkAllowance = _smsService.CheckAllowanceExistsForDesignation(model.DesignationId, model.Id);
            if (checkAllowance)
                ModelState.AddModelError("DesignationId", "A Allowance with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objAllowance = model.ToEntity();
                objAllowance.CreatedOn = objAllowance.ModifiedOn = DateTime.Now;
                objAllowance.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertAllowance(objAllowance);
                SuccessNotification("Allowance created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objAllowance.Id });
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
                model.AvailableDesignations = _smsService.GetAllDesignations().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.DesignationId == x.Id
                }).ToList();
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageAllowance"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteAllowance(id);

            SuccessNotification("Allowance deleted successfully.");
            return RedirectToAction("List");
        }

    }
}
