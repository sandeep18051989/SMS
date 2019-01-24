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
    public class FeeCategoryController : AdminAreaController
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

        public FeeCategoryController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var feeCategoryData = (from tempfeeCategory in _smsService.GetAllFeeCategories() select tempfeeCategory);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    feeCategoryData = feeCategoryData.Where(m => m.CategoryName.Contains(searchValue));
                }

                //total number of rows count     
                var lstFeeCategory = feeCategoryData as FeeCategory[] ?? feeCategoryData.ToArray();
                recordsTotal = lstFeeCategory.Count();
                //Paging     
                var data = lstFeeCategory.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new FeeCategoryModel()
                        {
                            AcadmicYearId = x.AcadmicYearId,
                            AcadmicYear = x.AcadmicYearId > 0 ? _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name : "",
                            Id = x.Id,
                            CategoryId = x.CategoryId,
                            CategoryName = x.CategoryId > 0 ? _smsService.GetCategoryById(x.CategoryId).Name : "",
                            ClassDivisionId = x.ClassDivisionId,
                            ClassDivisionName = x.ClassDivisionId > 0 ? (_smsService.GetClassRoomDivisionById(x.ClassDivisionId).Class.Name + " " + _smsService.GetClassRoomDivisionById(x.ClassDivisionId).Division.Name) : "",
                            FeeAmount = x.FeeAmount,
                            PeriodFrom = x.PeriodFrom,
                            PeriodTo = x.PeriodTo,
                            UserId = x.UserId,
                            StringPeriodFrom = x.PeriodFrom != null ? x.PeriodFrom.Value.ToString("U") : "",
                            StringPeriodTo = x.PeriodTo != null ? x.PeriodTo.Value.ToString("U") : ""
                        }).OrderBy(x => x.ClassDivisionName).ToList()
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

            var model = new FeeCategoryModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new FeeCategoryModel();
            var objFeeCategory = _smsService.GetFeeCategoryById(id);
            if (objFeeCategory != null)
            {
                model = objFeeCategory.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.AcadmicYearId > 0 ? model.AcadmicYearId == x.Id : false
            }).ToList();

            model.AvailableCategories = _smsService.GetAllCategories().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.CategoryId > 0 && model.CategoryId == x.Id
            }).OrderBy(x => x.Text).ToList();

            model.AvailableClassDivisions = _smsService.GetAllClassRoomDivisions().Select(x => new SelectListItem()
            {
                Text = x.Class.Name.Trim() + " " + x.Division.Name,
                Value = x.Id.ToString(),
                Selected = model.ClassDivisionId > 0 && model.ClassDivisionId == x.Id
            }).OrderBy(x => x.Text).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(FeeCategoryModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            if (model.CategoryId == 0 || model.ClassDivisionId == 0 || model.AcadmicYearId == 0)
            {
                if (model.CategoryId == 0)
                    ModelState.AddModelError("CategoryId", "Please select category");

                if (model.ClassDivisionId == 0)
                    ModelState.AddModelError("ClassDivisionId", "Please select class division");

                if (model.AcadmicYearId == 0)
                    ModelState.AddModelError("AcadmicYearId", "Please select acadmic year");

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId > 0 ? model.AcadmicYearId == x.Id : false
                }).ToList();

                model.AvailableCategories = _smsService.GetAllCategories().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.CategoryId > 0 && model.CategoryId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableClassDivisions = _smsService.GetAllClassRoomDivisions().Select(x => new SelectListItem()
                {
                    Text = x.Class.Name.Trim() + " " + x.Division.Name,
                    Value = x.Id.ToString(),
                    Selected = model.ClassDivisionId > 0 && model.ClassDivisionId == x.Id
                }).OrderBy(x => x.Text).ToList();
                return View(model);
            }

            // Check for duplicate classroom, if any
            var checkFeeCategory = _smsService.CheckFeeCategoryExists(model.CategoryId, model.ClassDivisionId, model.AcadmicYearId, model.Id);
            if (checkFeeCategory)
                ModelState.AddModelError("CategoryId", "A Fee Category with the same category, class division or acadmic year exist!");

            if (ModelState.IsValid)
            {
                var objFeeCategory = _smsService.GetFeeCategoryById(model.Id);
                if (objFeeCategory != null)
                {
                    model.CreatedOn = objFeeCategory.CreatedOn;
                    objFeeCategory = model.ToEntity(objFeeCategory);
                    objFeeCategory.ModifiedOn = DateTime.Now;
                    objFeeCategory.CategoryName = _smsService.GetCategoryById(model.CategoryId).Name;
                    _smsService.UpdateFeeCategory(objFeeCategory);
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

                model.AvailableCategories = _smsService.GetAllCategories().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.CategoryId > 0 && model.CategoryId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableClassDivisions = _smsService.GetAllClassRoomDivisions().Select(x => new SelectListItem()
                {
                    Text = x.Class.Name.Trim() + " " + x.Division.Name,
                    Value = x.Id.ToString(),
                    Selected = model.ClassDivisionId > 0 && model.ClassDivisionId == x.Id
                }).OrderBy(x => x.Text).ToList();
                return View(model);
            }

            SuccessNotification("FeeCategory updated successfully.");
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

            var model = new FeeCategoryModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
            }).ToList();

            model.AvailableCategories = _smsService.GetAllCategories().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
            }).OrderBy(x => x.Text).ToList();

            model.AvailableClassDivisions = _smsService.GetAllClassRoomDivisions().Select(x => new SelectListItem()
            {
                Text = x.Class.Name.Trim() + " " + x.Division.Name,
                Value = x.Id.ToString(),
            }).OrderBy(x => x.Text).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(FeeCategoryModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageFeeCategory"))
                return AccessDeniedView();

            if (model.CategoryId == 0 || model.ClassDivisionId == 0 || model.AcadmicYearId == 0)
            {
                if (model.CategoryId == 0)
                    ModelState.AddModelError("CategoryId", "Please select category");

                if (model.ClassDivisionId == 0)
                    ModelState.AddModelError("ClassDivisionId", "Please select class division");

                if (model.AcadmicYearId == 0)
                    ModelState.AddModelError("AcadmicYearId", "Please select acadmic year");

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId > 0 ? model.AcadmicYearId == x.Id : false
                }).ToList();

                model.AvailableCategories = _smsService.GetAllCategories().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.CategoryId > 0 && model.CategoryId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableClassDivisions = _smsService.GetAllClassRoomDivisions().Select(x => new SelectListItem()
                {
                    Text = x.Class.Name.Trim() + " " + x.Division.Name,
                    Value = x.Id.ToString(),
                    Selected = model.ClassDivisionId > 0 && model.ClassDivisionId == x.Id
                }).OrderBy(x => x.Text).ToList();
                return View(model);
            }

            // Check for duplicate classroom, if any
            var checkFeeCategory = _smsService.CheckFeeCategoryExists(model.CategoryId, model.ClassDivisionId, model.AcadmicYearId, model.Id);
            if (checkFeeCategory)
                ModelState.AddModelError("CategoryId", "A Fee Category with the same category, class division or acadmic year exist!");

            if (ModelState.IsValid)
            {
                var objFeeCategory = model.ToEntity();
                objFeeCategory.CreatedOn = objFeeCategory.ModifiedOn = DateTime.Now;
                objFeeCategory.UserId = _userContext.CurrentUser.Id;
                objFeeCategory.CategoryName = _smsService.GetCategoryById(model.CategoryId).Name;
                _smsService.InsertFeeCategory(objFeeCategory);
                SuccessNotification("Fee Category created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objFeeCategory.Id });
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

                model.AvailableCategories = _smsService.GetAllCategories().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.CategoryId > 0 && model.CategoryId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableClassDivisions = _smsService.GetAllClassRoomDivisions().Select(x => new SelectListItem()
                {
                    Text = x.Class.Name.Trim() + " " + x.Division.Name,
                    Value = x.Id.ToString(),
                    Selected = model.ClassDivisionId > 0 && model.ClassDivisionId == x.Id
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

            _smsService.DeleteFeeCategory(id);

            SuccessNotification("Fee Category deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusFeeCategory(id);
            ViewBag.Result = "Fee Category updated Successfully";

            return Json(new { Result = true });
        }

    }
}
