﻿using System;
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
    public class PaymentController : AdminAreaController
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

        public PaymentController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var paymentData = (from temppayment in _smsService.GetAllPayments() select temppayment);

                //total number of rows count     
                var lstPayment = paymentData as Payment[] ?? paymentData.ToArray();
                recordsTotal = lstPayment.Count();
                //Paging     
                var data = lstPayment.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new PaymentModel()
                        {
                            AcadmicYearId = x.AcadmicYearId,
                            AcadmicYear = x.AcadmicYearId > 0 ? _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name : "",
                            Id = x.Id,
                            UserId = x.UserId,
                            BasicPay = x.BasicPay,
                            DA = x.DA,
                            TA = x.TA,
                            Gross_Pay = x.Gross_Pay,
                            HRA = x.HRA,
                            TDS = x.TDS,
                            Net_Pay = x.Net_Pay,
                            PF = x.PF,
                            ESI = x.ESI,
                            EmployeeId = x.EmployeeId,
                            Employee = _smsService.GetEmployeeById(x.EmployeeId).EmpFName + "(" + _smsService.GetEmployeeById(x.EmployeeId).Username + ")",
                        }).OrderByDescending(x => x.CreatedOn).ToList()
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
            if (!_permissionService.Authorize("ManagePayment"))
                return AccessDeniedView();

            var model = new PaymentModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManagePayment"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new PaymentModel();
            var objPayment = _smsService.GetPaymentById(id);
            if (objPayment != null)
            {
                model = objPayment.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.AcadmicYearId > 0 ? model.AcadmicYearId == x.Id : false
            }).ToList();

            model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem()
            {
                Text = x.EmpFName.Trim() + (!string.IsNullOrEmpty(x.EmpMName) ? (" " + x.EmpMName) : "") + (!string.IsNullOrEmpty(x.EmpLName) ? (" " + x.EmpLName) : "") + ("(" + x.Username + ")"),
                Value = x.Id.ToString(),
                Selected = model.EmployeeId > 0 ? model.EmployeeId == x.Id : false
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(PaymentModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManagePayment"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var objFeedetail = _smsService.GetPaymentById(model.Id);
                if (objFeedetail != null)
                {
                    objFeedetail = model.ToEntity(objFeedetail);
                    objFeedetail.ModifiedOn = DateTime.Now;
                    _smsService.UpdatePayment(objFeedetail);
                }
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId > 0 ? model.AcadmicYearId == x.Id : false
                }).ToList();

                model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem()
                {
                    Text = x.EmpFName.Trim() + (!string.IsNullOrEmpty(x.EmpMName) ? (" " + x.EmpMName) : "") + (!string.IsNullOrEmpty(x.EmpLName) ? (" " + x.EmpLName) : "") + ("(" + x.Username + ")"),
                    Value = x.Id.ToString(),
                    Selected = model.EmployeeId > 0 ? model.EmployeeId == x.Id : false
                }).ToList();

                return View(model);
            }

            SuccessNotification("Payment updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManagePayment"))
                return AccessDeniedView();

            var model = new PaymentModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();

            model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem()
            {
                Text = x.EmpFName.Trim() + (!string.IsNullOrEmpty(x.EmpMName) ? (" " + x.EmpMName) : "") + (!string.IsNullOrEmpty(x.EmpLName) ? (" " + x.EmpLName) : "") + ("(" + x.Username + ")"),
                Value = x.Id.ToString(),
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(PaymentModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManagePayment"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var objPayment = model.ToEntity();
                objPayment.CreatedOn = objPayment.ModifiedOn = DateTime.Now;
                objPayment.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertPayment(objPayment);
                SuccessNotification("Payment created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objPayment.Id });
                }
            }
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId > 0 ? model.AcadmicYearId == x.Id : false
                }).ToList();

                model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem()
                {
                    Text = x.EmpFName.Trim() + (!string.IsNullOrEmpty(x.EmpMName) ? (" " + x.EmpMName) : "") + (!string.IsNullOrEmpty(x.EmpLName) ? (" " + x.EmpLName) : "") + ("(" + x.Username + ")"),
                    Value = x.Id.ToString(),
                    Selected = model.EmployeeId > 0 ? model.EmployeeId == x.Id : false
                }).ToList();
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManagePayment"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _roleService.Delete(id);

            SuccessNotification("Payment deleted successfully.");
            return RedirectToAction("List");
        }
    }
}