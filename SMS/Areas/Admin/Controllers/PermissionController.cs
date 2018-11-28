using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class PermissionController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IPermissionService _permissionService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;
		private readonly IRoleService _roleService;

		#endregion Fileds

		#region Constructor

		public PermissionController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IPermissionService permissionService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IRoleService roleService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._permissionService = permissionService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
			this._roleService = roleService;
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
	            var userData = (from temppermissions in _permissionService.GetAllPermissions(showSystemDefined:false) select temppermissions);

	            //Search    
	            if (!string.IsNullOrEmpty(searchValue))
	            {
	                userData = userData.Where(m => m.Name.Contains(searchValue) || m.Category.Contains(searchValue) || m.SystemName.Contains(searchValue));
	            }

	            //total number of rows count     
	            var lstUsers = userData as PermissionRecord[] ?? userData.ToArray();
	            recordsTotal = lstUsers.Count();
	            //Paging     
	            var data = lstUsers.Skip(skip).Take(pageSize);

	            //Returning Json Data 
	            return new JsonResult()
	            {
	                Data = new
	                {
	                    draw = draw,
	                    recordsFiltered = recordsTotal,
	                    recordsTotal = recordsTotal,
	                    data = data.Select(x => new PermissionRecordModel()
	                    {
	                        IsActive = x.IsActive,
	                        UserId = x.UserId,
	                        Category = x.Category.Trim(),
                            IsSystemDefined = x.IsSystemDefined,
                            Name = x.Name.Trim(),
                            SystemName = x.SystemName.Trim(),
	                        Id = x.Id
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

        #region Security Actions

        public ActionResult AccessDenied()
		{
			var currentUser = _userContext.CurrentUser;
			if (currentUser == null && !_userContext.IsAdmin)
			{
				//_logger.Information(string.Format("Access denied to anonymous request on {0}", pageUrl));
				return View();
			}

			//_logger.Information(string.Format("Access denied to user #{0} '{1}' on {2}", currentCustomer.Email, currentCustomer.Email, pageUrl));


			return View();
		}


		#endregion
		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			var model = new PermissionRecordModel();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			var model = new PermissionRecordModel();
			if (id == 0)
				throw new Exception("Permission Id Missing");

			var eve = _permissionService.GetPermissionById(id);
			if (eve != null)
			{
				model = new PermissionRecordModel
                {
					Id = id,
					Name = eve.Name,
                    IsActive = eve.IsActive,
					IsSystemDefined = eve.IsSystemDefined,
					Category = eve.Category
				};
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(PermissionRecordModel model)
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			// Check for duplicate permission, if any
			var permission = _permissionService.GetPermissionsByName(model.Name.Trim());
			if (permission != null && permission.Id != model.Id)
				ModelState.AddModelError("PermissionName", "A Permission with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var eve = _permissionService.GetPermissionById(model.Id);
				if (eve != null)
				{
					eve.ModifiedOn = DateTime.Now;
					eve.Name = model.Name;
					eve.IsActive = model.IsActive;
					eve.Category = model.Category;
					_permissionService.Update(eve);
				}
			}
			else
			{
				return View(model);
			}

			SuccessNotification("Permission updated successfully.");
			return RedirectToAction("PermissionsList");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			var model = new PermissionRecordModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(PermissionRecordModel model)
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			// Check for duplicate permission, if any
			var permission = _permissionService.GetPermissionsByName(model.Name);
			if (permission != null)
				ModelState.AddModelError("PermissionName", "A Permission with same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var newPermission = new PermissionRecord()
				{
					CreatedOn = DateTime.Now,
					IsSystemDefined = false,
					ModifiedOn = DateTime.Now,
					IsDeleted = false,
					Name = model.Name,
                    IsActive = model.IsActive
				};
				_permissionService.Insert(newPermission);
			}
			else
			{
				return View(model);
			}

			SuccessNotification("Permission created successfully.");
			return View(model);
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			if (!_permissionService.GetPermissionById(id).IsSystemDefined)
			{
				_permissionService.Delete(id);
			}

			SuccessNotification("Permission deleted successfully.");
			return RedirectToAction("PermissionsList");
		}

		[HttpPost]
		public ActionResult DeleteSelected(ICollection<int> selectedIds)
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			if (selectedIds != null)
			{
				_permissionService.DeletePermissions(_permissionService.GetPermissionsByIds(selectedIds.ToArray()).ToList());
			}

			return Json(new { Result = true });
		}

		public ActionResult RolePermissions()
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			var model = new RolePermissionModel();
			// Load Available Roles
			model.AvailableRoles.Add(new SelectListItem { Text = "-- Select Role --", Value = "0" });
			foreach (var t in _roleService.GetAllRoles().Where(x => x.IsActive == true).ToList())
				model.AvailableRoles.Add(new SelectListItem { Text = t.RoleName, Value = t.Id.ToString() });

			return View(model);
		}

		[ValidateInput(false)]
		public ActionResult GetRolePermissions(int id)
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			var model = new RolePermissionModel();

			if (id == 0)
				throw new Exception("Role Id Missing");

			var allPermissions = _permissionService.GetAllPermissions(true).Select(x => new PermissionModel()
			{
				Category = x.Category,
				Id = x.Id,
				IsSystemDefined = x.IsSystemDefined,
				Name = x.Name,
				SystemName = x.SystemName,
			}).ToList();

			var roles = _roleService.GetAllRoles(true);
			if (roles.Count > 0)
			{
				// Load Available Roles
				model.AvailableRoles.Add(new SelectListItem { Text = "-- Select Role --", Value = "0" });
				foreach (var t in roles)
					model.AvailableRoles.Add(new SelectListItem { Text = t.RoleName, Value = t.Id.ToString() });
			}

			var permissions = _permissionService.GetPermissionsByRoleId(id);

			// Prepare All Permissions
			if (allPermissions.Count > 0)
			{
				foreach (var permit in allPermissions)
				{
					var permissionChecked = new PermissionModel
					{
						Id = permit.Id,
						Name = permit.Name,
						SystemName = permit.SystemName,
						IsSystemDefined = permit.IsSystemDefined,
						Category = permit.Category
					};
					model.AvailablePermissions.Add(new PermissionModel()
					{
						Category = permit.Category,
						Id = permit.Id,
						IsSystemDefined = permit.IsSystemDefined,
						Name = permit.Name,
						SystemName = permit.SystemName
					});
				}

				if (permissions != null)
				{
					foreach (var per in permissions)
					{
						var checkPermission = model.AvailablePermissions.FirstOrDefault(x => x.Id == per.Id);
						if (checkPermission != null)
						{
							checkPermission.Checked = true;
						}
					}
				}
			}
			model.RoleId = id;
			return PartialView("~/Areas/Admin/Views/Shared/_RolePermissionList.cshtml", model);
		}

		public ActionResult RolePermissionList(int id)
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			var model = new RolePermissionModel();
			if (id == 0)
				throw new Exception("Role Id Missing");

			var allPermissions = _permissionService.GetAllPermissions(true).Select(x => new PermissionModel()
			{
				Category = x.Category,
				Id = x.Id,
				IsSystemDefined = x.IsSystemDefined,
				Name = x.Name,
				SystemName = x.SystemName,
			}).ToList();

			var roles = _roleService.GetAllRoles(true);
			if (roles.Count > 0)
			{
				// Load Available Templates
				model.AvailableRoles.Add(new SelectListItem { Text = "-- Select Role --", Value = "0" });
				foreach (var t in roles)
					model.AvailableRoles.Add(new SelectListItem { Text = t.RoleName, Value = t.Id.ToString() });
			}

			var permissions = _permissionService.GetPermissionsByRoleId(id);

			// Prepare All Permissions
			if (allPermissions.Count > 0)
			{
				foreach (var permit in allPermissions)
				{
					var permissionChecked = new PermissionModel
					{
						Id = permit.Id,
						Name = permit.Name,
						SystemName = permit.SystemName,
						IsSystemDefined = permit.IsSystemDefined,
						Category = permit.Category
					};
					model.AvailablePermissions.Add(new PermissionModel()
					{
						Category = permit.Category,
						Id = permit.Id,
						IsSystemDefined = permit.IsSystemDefined,
						Name = permit.Name,
						SystemName = permit.SystemName
					});
				}

				if (permissions != null)
				{
					foreach (var per in permissions)
					{
						var checkPermission = model.AvailablePermissions.FirstOrDefault(x => x.Id == per.Id);
						if (checkPermission != null)
						{
							checkPermission.Checked = true;
						}
					}
				}
			}
			model.RoleId = id;
			return View("~/Areas/Admin/Views/Permission/RolePermissions.cshtml", model);
		}

		[HttpPost]
		public ActionResult RolePermissions(RolePermissionModel model)
		{
			if (!_permissionService.Authorize("ManagePermissions"))
				return AccessDeniedView();

			var newpermissions = new List<PermissionRecord>();
			if (ModelState.IsValid)
			{
				var permissions = _permissionService.GetAllPermissions(true);
				var selectedRole = _roleService.GetRoleById(model.RoleId);

				foreach (var permit in permissions)
				{
					if (model.Selectedpermissions.Contains(permit.Id))
					{
						newpermissions.Add(permit);
					}
				}

				// Permissions
				foreach (var permit in newpermissions)
				{
					if (model.Selectedpermissions.Contains(permit.Id))
					{
						//new
						if (selectedRole.PermissionRecords.Count(cr => cr.Id == permit.Id) == 0)
							selectedRole.PermissionRecords.Add(permit);
					}
					else
					{
						//remove
						if (selectedRole.PermissionRecords.Count(cr => cr.Id == permit.Id) > 0)
							selectedRole.PermissionRecords.Remove(permit);
					}
				}

				_roleService.Update(selectedRole);
			}

			model.AvailablePermissions = _permissionService.GetAllPermissions(true).Select(x => new PermissionModel()
			{
				Name = x.Name,
				Id = x.Id,
				Category = x.Category,
				Checked = newpermissions.Any(p => p.Id == x.Id)
			}).ToList();



			SuccessNotification("Role Permissions updated successfully.");
			model.AvailableRoles.Clear();
			var roles = _roleService.GetAllRoles(true);
			if (roles.Count <= 0) return View("~/Areas/Admin/Views/Role/List.cshtml");
			// Load Available Templates
			model.AvailableRoles.Add(new SelectListItem { Text = "-- Select Role --", Value = "0" });
			foreach (var t in roles)
				model.AvailableRoles.Add(new SelectListItem { Text = t.RoleName, Value = t.Id.ToString() });

			return View("~/Areas/Admin/Views/Permission/RolePermissions.cshtml", model);
		}

	}
}
