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
    public class ClassRoomController : AdminAreaController
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

        public ClassRoomController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var classroomData = (from tempclassroomes in _smsService.GetAllClassRooms() select tempclassroomes);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    classroomData = classroomData.Where(m => m.Number.Contains(searchValue) || m.Description.Contains(searchValue));
                }

                //total number of rows count     
                var lstClassRoomes = classroomData as ClassRoom[] ?? classroomData.ToArray();
                recordsTotal = lstClassRoomes.Count();
                //Paging     
                var data = lstClassRoomes.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new ClassRoomModel()
                        {
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            Number = x.Number.Trim().ToUpper(),
                            AcadmicYearId = x.AcadmicYearId,
                            AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name,
                            CreatedOnString = x.CreatedOn.HasValue ? x.CreatedOn.Value.ToString("U") : "",
                            ModifiedOnString = x.ModifiedOn.HasValue ? x.ModifiedOn.Value.ToString("U") : "",
                            Description = x.Description,
                            Id = x.Id
                        }).OrderBy(x => x.Number).ToList()
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
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            var model = new ClassRoomModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new ClassRoomModel();
            var objClassRoom = _smsService.GetClassRoomById(id);
            if (objClassRoom != null)
            {
                model = objClassRoom.ToModel();
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
        public ActionResult Edit(ClassRoomModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkClassRoom = _smsService.CheckClassRoomExists(model.Number, model.Id);
            if (checkClassRoom)
                ModelState.AddModelError("Number", "A ClassRoom with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objClassRoom = _smsService.GetClassRoomById(model.Id);
                if (objClassRoom != null)
                {
                    model.CreatedOn = objClassRoom.CreatedOn;
                    objClassRoom = model.ToEntity(objClassRoom);
                    objClassRoom.ModifiedOn = DateTime.Now;
                    _smsService.UpdateClassRoom(objClassRoom);
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

            SuccessNotification("ClassRoom updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            var model = new ClassRoomModel();
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
        public ActionResult Create(ClassRoomModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkClassRoom = _smsService.CheckClassRoomExists(model.Number, model.Id);
            if (checkClassRoom)
                ModelState.AddModelError("Number", "A ClassRoom with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objClassRoom = model.ToEntity();
                objClassRoom.CreatedOn = objClassRoom.ModifiedOn = DateTime.Now;
                objClassRoom.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertClassRoom(objClassRoom);
                SuccessNotification("ClassRoom created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objClassRoom.Id });
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
            if (!_permissionService.Authorize("ManageClassRoom"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteClassRoom(id);

            SuccessNotification("ClassRoom deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusClassRoom(id);
            ViewBag.Result = "ClassRoom updated Successfully";

            return Json(new { Result = true });
        }

    }
}
