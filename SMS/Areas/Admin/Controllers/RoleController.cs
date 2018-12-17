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

namespace SMS.Areas.Admin.Controllers
{
    public class RoleController : AdminAreaController
    {

        #region Fields

        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IUserContext _userContext;
        private readonly ISliderService _sliderService;
        private readonly ISettingService _settingService;
        private readonly IRoleService _roleService;
        private readonly IVideoService _videoService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;
        private readonly IBlogService _blogService;
        private readonly IPermissionService _permissionService;

        #endregion Fileds

        #region Constructor

        public RoleController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IRoleService roleService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IPermissionService permissionService)
        {
            this._userService = userService;
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._sliderService = sliderService;
            this._settingService = settingService;
            this._roleService = roleService;
            this._videoService = videoService;
            this._commentService = commentService;
            this._replyService = replyService;
            this._blogService = blogService;
            this._permissionService = permissionService;
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
                var roleData = (from temproles in _roleService.GetAllRoles(showSystemDefined: true) select temproles);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    roleData = roleData.Where(m => m.RoleName.Contains(searchValue));
                }

                //total number of rows count     
                var lstRoles = roleData as UserRole[] ?? roleData.ToArray();
                recordsTotal = lstRoles.Count();
                //Paging     
                var data = lstRoles.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new RoleModel()
                        {
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            RoleName = x.RoleName.Trim(),
                            IsSystemDefined = x.IsSystemDefined,
                            Id = x.Id
                        }).OrderBy(x => x.RoleName).ToList()
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
            if (!_permissionService.Authorize("ManageRoles"))
                return AccessDeniedView();
            var model = new RoleModel();

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = new RoleModel();
            if (!_permissionService.Authorize("ManageRoles"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Role Id Missing");

            var role = _roleService.GetRoleById(id);
            if (role != null)
            {
                model = new RoleModel();
                model = role.ToModel();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(RoleModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageRoles"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate role, if any
            var role = _roleService.GetRoleByName(model.RoleName);
            if (role != null && role.Id != model.Id)
                ModelState.AddModelError("RoleName", "A Role with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var eve = _roleService.GetRoleById(model.Id);
                if (eve != null)
                {
                    eve.RoleName = model.RoleName;
                    eve.IsActive = model.IsActive;
                    eve.ModifiedOn = DateTime.Now;
                    _roleService.Update(eve);
                }
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Role updated successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageRoles"))
                return AccessDeniedView();

            var model = new RoleModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(RoleModel model)
        {
            if (!_permissionService.Authorize("ManageRoles"))
                return AccessDeniedView();

            // Check for duplicate role, if any
            var role = _roleService.GetRoleByName(model.RoleName);
            if (role != null)
                ModelState.AddModelError("RoleName", "An Role with same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var newRole = new UserRole()
                {
                    CreatedOn = DateTime.Now,
                    IsActive = model.IsActive,
                    ModifiedOn = DateTime.Now,
                    RoleName = model.RoleName
                };
                _roleService.Insert(newRole);
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Role created successfully.");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageRoles"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            if (!_roleService.GetRoleById(id).IsSystemDefined)
            {
                _roleService.Delete(id);
            }

            SuccessNotification("Role deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (user != null && user.Roles.Any(r => r.Id == 1))
            {
                if (id == 0)
                    throw new Exception("Id Not Found");

                if (!_roleService.GetRoleById(id).IsSystemDefined)
                {
                    _roleService.ToggleRole(id);
                    ViewBag.Result = "Role Status Changed Successfully";
                }

                return Json(new { Result = true });
            }
            else
            {
                return Json(new { Result = false });
            }
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize("ManageRoles"))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _roleService.DeleteRoles(_roleService.GetRolesByIds(selectedIds.ToArray()).ToList());
            }

            SuccessNotification("Roles deleted successfully.");
            return RedirectToAction("List");
        }

    }
}
