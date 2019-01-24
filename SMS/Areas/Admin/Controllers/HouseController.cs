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
    public class HouseController : AdminAreaController
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
        private readonly IPictureService _pictureService;

        #endregion Fileds

        #region Constructor

        public HouseController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService, IPictureService pictureService)
        {
            this._userService = userService;
            this._userContext = userContext;
            this._settingService = settingService;
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._smsService = smsService;
            this._commentService = commentService;
            this._replyService = replyService;
            this._pictureService = pictureService;
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
                var HouseData = (from tempHouse in _smsService.GetAllHouses() select tempHouse);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    HouseData = HouseData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstHouse = HouseData as House[] ?? HouseData.ToArray();
                recordsTotal = lstHouse.Count();
                //Paging     
                var data = lstHouse.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new HouseModel()
                        {
                            AcadmicYearId = x.AcadmicYearId,
                            Id = x.Id,
                            Name = x.Name,
                            IsActive = x.IsActive,
                            AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId).Name,
                            Description = x.Description,
                            PictureSrc = _pictureService.GetPictureById(x.PictureId).PictureSrc
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
            if (!_permissionService.Authorize("ManageHouse"))
                return AccessDeniedView();

            var model = new HouseModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageHouse"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new HouseModel();
            var objHouse = _smsService.GetHouseById(id);
            if (objHouse != null)
            {
                model = objHouse.ToModel();
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
        public ActionResult Edit(HouseModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageHouse"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkHouse = _smsService.CheckHouseExists(model.Name, model.Id);
            if (checkHouse)
                ModelState.AddModelError("Name", "A House with the same name already exists!");

            if (ModelState.IsValid)
            {
                var objHouse = _smsService.GetHouseById(model.Id);
                if (objHouse != null)
                {
                    model.CreatedOn = objHouse.CreatedOn;
                    objHouse = model.ToEntity(objHouse);
                    objHouse.ModifiedOn = DateTime.Now;
                    _smsService.UpdateHouse(objHouse);
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

            SuccessNotification("House updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageHouse"))
                return AccessDeniedView();

            var model = new HouseModel();
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
        public ActionResult Create(HouseModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageHouse"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkHouse = _smsService.CheckHouseExists(model.Name, model.Id);
            if (checkHouse)
                ModelState.AddModelError("Name", "A House with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objHouse = model.ToEntity();
                objHouse.CreatedOn = objHouse.ModifiedOn = DateTime.Now;
                objHouse.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertHouse(objHouse);
                SuccessNotification("House created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objHouse.Id });
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
            if (!_permissionService.Authorize("ManageHouse"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteHouse(id);

            SuccessNotification("House deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusHouse(id);
            ViewBag.Result = "House updated Successfully";

            return Json(new { Result = true });
        }

    }
}
