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
    public class DivisionController : AdminAreaController
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

        public DivisionController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var divisionData = (from tempdivisiones in _smsService.GetAllDivisions() select tempdivisiones);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    divisionData = divisionData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstDivisiones = divisionData as Division[] ?? divisionData.ToArray();
                recordsTotal = lstDivisiones.Count();
                //Paging     
                var data = lstDivisiones.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new DivisionModel()
                        {
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            Name = x.Name.Trim(),
                            AcadmicYearId = x.AcadmicYearId,
                            AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name,
                            CreatedOnString = x.CreatedOn.ToString("U"),
                            ModifiedOnString = x.ModifiedOn.ToString("U"),
                            Description = x.Description,
                            DisplayOrder = x.DisplayOrder,
                            Id = x.Id
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

        public JsonResult GetAllDivisions(int? classid=null)
        {
            var allDivisions = _smsService.GetAllDivisions();
            var divisionsByClass = _smsService.GetAllDivisionsByClass(id: classid, onlyActive: true);

            //Returning Json Data 
            return new JsonResult()
            {
                Data = allDivisions.Select(x => new DivisionModel()
                {
                    IsActive = x.IsActive,
                    UserId = x.UserId,
                    Name = x.Name.Trim(),
                    AcadmicYearId = x.AcadmicYearId,
                    AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name,
                    CreatedOnString = x.CreatedOn.ToString("U"),
                    ModifiedOnString = x.ModifiedOn.ToString("U"),
                    Description = x.Description,
                    DisplayOrder = x.DisplayOrder,
                    Id = x.Id,
                    Selected = divisionsByClass.Any(y => y.DivisionId == x.Id)
                }).OrderBy(x => x.DisplayOrder).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion
        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            var model = new DivisionModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new DivisionModel();
            var objDivision = _smsService.GetDivisionById(id);
            if (objDivision != null)
            {
                model = objDivision.ToModel();
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
        public ActionResult Edit(DivisionModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate division, if any
            var checkDivision = _smsService.CheckDivisionExists(model.Name, model.Id);
            if (checkDivision)
                ModelState.AddModelError("Name", "A Division with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objDivision = _smsService.GetDivisionById(model.Id);
                if (objDivision != null)
                {
                    model.CreatedOn = objDivision.CreatedOn;
                    objDivision = model.ToEntity(objDivision);
                    objDivision.ModifiedOn = DateTime.Now;
                    _smsService.UpdateDivision(objDivision);
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

            SuccessNotification("Division updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            var model = new DivisionModel();
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
        public ActionResult Create(DivisionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            // Check for duplicate division, if any
            var checkDivision = _smsService.CheckDivisionExists(model.Name, model.Id);
            if (checkDivision)
                ModelState.AddModelError("Name", "A Division with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objDivision = model.ToEntity();
                objDivision.CreatedOn = objDivision.ModifiedOn = DateTime.Now;
                objDivision.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertDivision(objDivision);
                SuccessNotification("Division created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objDivision.Id });
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
            if (!_permissionService.Authorize("ManageDivision"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteDivision(id);

            SuccessNotification("Division deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusDivision(id);
            ViewBag.Result = "Division updated Successfully";

            return Json(new { Result = true });
        }

    }
}
