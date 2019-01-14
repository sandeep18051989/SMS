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
    public class FeeDetailController : AdminAreaController
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

        public FeeDetailController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var feeDetailData = (from tempfeeDetail in _smsService.GetAllFeeDetails() select tempfeeDetail);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    feeDetailData = feeDetailData.Where(m => m.CashierName.Contains(searchValue) || m.Student.UserName.Contains(searchValue));
                }

                //total number of rows count     
                var lstFeeDetail = feeDetailData as FeeDetail[] ?? feeDetailData.ToArray();
                recordsTotal = lstFeeDetail.Count();
                //Paging     
                var data = lstFeeDetail.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new FeeDetailModel()
                        {
                            AcadmicYearId = x.AcadmicYearId,
                            AcadmicYear = x.AcadmicYearId > 0 ? _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name : "",
                            Id = x.Id,
                            BankName = x.BankName,
                            CashierId = x.CashierId,
                            CashierName = _smsService.GetEmployeeById(x.CashierId).EmpFName + "(" + _smsService.GetEmployeeById(x.CashierId).Username + ")",
                            DDChequeNumber = x.DDChequeNumber,
                            FeeCategoryStructureId = x.FeeCategoryStructureId,
                            FeesPaid = x.FeesPaid,
                            FeeType = x.FeeType,
                            PaidBy = x.PaidBy,
                            PayingMode = x.PayingMode,
                            Remarks = x.Remarks,
                            StatusId = x.StatusId,
                            StudentId = x.StudentId,
                            TotalFees = x.TotalFees,
                            UserId = x.UserId,
                            Student = _smsService.GetStudentById(x.StudentId).FName + "(" + _smsService.GetStudentById(x.StudentId).UserName + ")",
                            Status = x.StatusId > 0 ? Enum.GetValues(typeof(FeeStatus)).GetValue(x.StatusId).ToString() : "",
                            StringDate = x.Date.Value.ToString("U")
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
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            var model = new FeeDetailModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new FeeDetailModel();
            var objFeeDetail = _smsService.GetFeeDetailById(id);
            if (objFeeDetail != null)
            {
                model = objFeeDetail.ToModel();
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
                Selected = model.CashierId > 0 ? model.CashierId == x.Id : false
            }).ToList();

            model.AvailableStudents = _smsService.GetAllStudents().Select(x => new SelectListItem()
            {
                Text = x.FName.Trim() + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : "") + ("(" + x.UserName + ")"),
                Value = x.Id.ToString(),
                Selected = model.StudentId == x.Id
            }).ToList();

            model.AvailableStatuses = (from FeeStatus d in Enum.GetValues(typeof(FeeStatus))
                                           select new SelectListItem
                                           {
                                               Text = d.ToString(),
                                               Value = Convert.ToInt32(d).ToString(),
                                               Selected = (Convert.ToInt32(d) == model.StatusId)
                                           }).ToList();

            model.AvailableFeeCategoryStructures = _smsService.GetAllFeeCategories().Select(x => new SelectListItem()
            {
                Text = x.CategoryName.Trim() + " (" + x.ClassDivision.Class.Name + " " + x.ClassDivision.Division.Name + ")",
                Value = x.Id.ToString(),
                Selected = model.FeeCategoryStructureId > 0 && model.FeeCategoryStructureId == x.Id
            }).OrderBy(x => x.Text).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(FeeDetailModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var objFeedetail = _smsService.GetFeeDetailById(model.Id);
                if (objFeedetail != null)
                {
                    objFeedetail = model.ToEntity(objFeedetail);
                    objFeedetail.ModifiedOn = DateTime.Now;
                    objFeedetail.CashierName = _smsService.GetEmployeeById(model.CashierId).EmpFName + "(" + _smsService.GetEmployeeById(model.CashierId).Username + ")";
                    _smsService.UpdateFeeDetail(objFeedetail);
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
                    Selected = model.CashierId > 0 ? model.CashierId == x.Id : false
                }).ToList();

                model.AvailableStudents = _smsService.GetAllStudents().Select(x => new SelectListItem()
                {
                    Text = x.FName.Trim() + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : "") + ("(" + x.UserName + ")"),
                    Value = x.Id.ToString(),
                    Selected = model.StudentId == x.Id
                }).ToList();

                model.AvailableStatuses = (from FeeStatus d in Enum.GetValues(typeof(FeeStatus))
                                           select new SelectListItem
                                           {
                                               Text = d.ToString(),
                                               Value = Convert.ToInt32(d).ToString(),
                                               Selected = (Convert.ToInt32(d) == model.StatusId)
                                           }).ToList();

                model.AvailableFeeCategoryStructures = _smsService.GetAllFeeCategories().Select(x => new SelectListItem()
                {
                    Text = x.CategoryName.Trim() + " (" + x.ClassDivision.Class.Name + " " + x.ClassDivision.Division.Name + ")",
                    Value = x.Id.ToString(),
                    Selected = model.FeeCategoryStructureId > 0 && model.FeeCategoryStructureId == x.Id
                }).OrderBy(x => x.Text).ToList();
                return View(model);
            }

            SuccessNotification("Fee Detail updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            var model = new FeeDetailModel();
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

            model.AvailableStudents = _smsService.GetAllStudents().Select(x => new SelectListItem()
            {
                Text = x.FName.Trim() + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : "") + ("(" + x.UserName + ")"),
                Value = x.Id.ToString(),
            }).ToList();

            model.AvailableStatuses = (from FeeStatus d in Enum.GetValues(typeof(FeeStatus))
                                       select new SelectListItem
                                       {
                                           Text = d.ToString(),
                                           Value = Convert.ToInt32(d).ToString(),
                                       }).ToList();

            model.AvailableFeeCategoryStructures = _smsService.GetAllFeeCategories().Select(x => new SelectListItem()
            {
                Text = x.CategoryName.Trim() + " (" + x.ClassDivision.Class.Name + " " + x.ClassDivision.Division.Name + ")",
                Value = x.Id.ToString(),
            }).OrderBy(x => x.Text).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(FeeDetailModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var objFeeDetail = model.ToEntity();
                objFeeDetail.CreatedOn = objFeeDetail.ModifiedOn = DateTime.Now;
                objFeeDetail.UserId = _userContext.CurrentUser.Id;
                objFeeDetail.CashierName = _smsService.GetEmployeeById(model.CashierId).EmpFName + "(" + _smsService.GetEmployeeById(model.CashierId).Username + ")";
                _smsService.InsertFeeDetail(objFeeDetail);
                SuccessNotification("Fee Detail created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objFeeDetail.Id });
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
                    Selected = model.CashierId > 0 ? model.CashierId == x.Id : false
                }).ToList();

                model.AvailableStudents = _smsService.GetAllStudents().Select(x => new SelectListItem()
                {
                    Text = x.FName.Trim() + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : "") + ("(" + x.UserName + ")"),
                    Value = x.Id.ToString(),
                    Selected = model.StudentId == x.Id
                }).ToList();

                model.AvailableStatuses = (from FeeStatus d in Enum.GetValues(typeof(FeeStatus))
                                           select new SelectListItem
                                           {
                                               Text = d.ToString(),
                                               Value = Convert.ToInt32(d).ToString(),
                                               Selected = (Convert.ToInt32(d) == model.StatusId)
                                           }).ToList();

                model.AvailableFeeCategoryStructures = _smsService.GetAllFeeCategories().Select(x => new SelectListItem()
                {
                    Text = x.CategoryName.Trim() + " (" + x.ClassDivision.Class.Name + " " + x.ClassDivision.Division.Name + ")",
                    Value = x.Id.ToString(),
                    Selected = model.FeeCategoryStructureId > 0 && model.FeeCategoryStructureId == x.Id
                }).OrderBy(x => x.Text).ToList();
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _roleService.Delete(id);

            SuccessNotification("Fee Detail deleted successfully.");
            return RedirectToAction("List");
        }
    }
}
