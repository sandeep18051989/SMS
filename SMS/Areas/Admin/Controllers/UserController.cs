using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
	public class UserController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		public readonly ISettingService _settingService;
		public readonly IRoleService _roleService;
		private readonly IPermissionService _permissionService;
		private readonly IAuditService _auditService;

		#endregion Fileds

		#region Constructor

		public UserController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, IAuditService auditService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._roleService = roleService;
			this._permissionService = permissionService;
			this._auditService = auditService;
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
	            var userData = (from tempusers in _userService.GetAllUsers().Where(x => x.Id != 1) select tempusers);

	            //Search    
	            if (!string.IsNullOrEmpty(searchValue))
	            {
	                userData = userData.Where(m => m.SeoName.Contains(searchValue) || m.UserName.Contains(searchValue));
	            }

	            //total number of rows count     
	            var lstUsers = userData as User[] ?? userData.ToArray();
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
	                    data = data.Select(x => new UserModel()
	                    {
	                      IsActive  = x.IsActive,
                          UserId = x.UserId,
                          Username = x.UserName,
                          AvailableRoles = x.Roles.Select(y => new RoleModel()
                          {
                              RoleName = y.RoleName
                          }).ToList(),
                          IsApproved = x.IsApproved,
                          Id = x.Id
	                    }).OrderBy(x => x.Username).ToList()
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

        #region Action Methods

        public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			var model = new List<UserModel>();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			var model = new UserModel();
			if (id == 0)
				throw new Exception("User Id Missing");

			if (id == 1)
				throw new Exception("Administrator cannot be edited.");

			var user = _userService.GetUserById(id);
			if (user != null)
			{
				model = new UserModel
				{
					Id = id,
					IsActive = user.IsActive,
					CreatedOn = user.CreatedOn,
					IsApproved = user.IsApproved,
					ChangePassword = new ChangePasswordModel() { Id = user.Id, OldPassword = user.Password },
					AvailableRoles = _roleService.GetAllRoles().Select(r => new RoleModel()
					{
						Id = r.Id,
						IsActive = r.IsActive,
						IsDeleted = r.IsDeleted,
						IsSystemDefined = r.IsSystemDefined,
						RoleName = r.RoleName,
					}).ToList(),
					Username = user.UserName,
					Email = user.Email,
					UserGuid = user.UserGuid,
					ModifiedOn = user.ModifiedOn
				};
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(UserModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			// Check for duplicate user, if any
			var allActiveUsers = _userService.GetAllUsers(true, false);

			if (allActiveUsers.Any(u => u.UserName == model.Username && u.Id != model.Id))
				ModelState.AddModelError("UserName", "A User with the same name already exists. Please choose a different name.");

			var user = _userService.GetUserById(model.Id);

			if (ModelState.IsValid)
			{
				var availableRoles = _roleService.GetAllRoles();
				if (user != null && availableRoles.Count > 0)
				{
					user.UserName = model.Username;
					user.IsActive = model.IsActive;
					user.ModifiedOn = DateTime.Now;

					if (frm.Keys.Count > 0)
					{
						user.Roles.Clear();
						foreach (string key in frm.AllKeys.Where(x => availableRoles.Any(r => r.RoleName == x)).ToList())
						{
							var assignedRole = availableRoles.FirstOrDefault(ar => ar.RoleName == key);
							if (assignedRole != null)
							{
								user.Roles.Add(assignedRole);
							}
						}
						_userService.Update(user);

					}
				}
				else
				{
					return View(model);
				}
			}

			SuccessNotification("User updated successfully.");
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			var model = new UserModel();
			model.AvailableRoles = _roleService.GetAllRoles().Select(x => new RoleModel()
			{
				Id = x.Id,
				IsActive = x.IsActive,
				IsSystemDefined = x.IsSystemDefined,
				RoleName = x.RoleName
			}).ToList();
			return View(model);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(UserModel model)
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			// Check for duplicate user, if any

			var userbyname = _userService.GetUserByUsername(model.Username);
			if (userbyname != null)
				ModelState.AddModelError("UserName", "Username already exists. Please choose a different name.");

			var userbyemail = _userService.GetUserByEmail(model.Email);
			if (userbyemail != null)
				ModelState.AddModelError("UserName", "Email address already exists. Please choose a different email address.");

			if (ModelState.IsValid)
			{
				var newUser = new User()
				{
					CreatedOn = DateTime.Now,
					IsActive = model.IsActive,
					ModifiedOn = DateTime.Now,
					UserName = model.Username,
					IsDeleted = false,
					UserGuid = new Guid(),
					IsApproved = model.IsApproved,
					Email = model.Email,
					Password = model.Password,
					UserId = _userContext.CurrentUser.Id
				};

				if (model.SelectedRoleIds.Length > 0)
				{
					foreach (var id in model.SelectedRoleIds)
					{
						var role = _roleService.GetRoleById(id);
						if (role != null)
							newUser.Roles.Add(role);
					}
				}

				_userService.Insert(newUser);
			}
			else
			{
				model.AvailableRoles = _roleService.GetAllRoles().Select(x => new RoleModel()
				{
					Id = x.Id,
					IsActive = x.IsActive,
					IsSystemDefined = x.IsSystemDefined,
					RoleName = x.RoleName
				}).ToList();
				return View(model);
			}

			SuccessNotification("User created successfully.");
			return RedirectToAction("List");
		}

		public ActionResult ApproveUser(int id)
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			var _user = _userService.GetUserById(id);
			if (_userContext.IsAdmin)
			{
				var _usrList = new List<User>();
				_usrList.Add(_user);
				_userService.ApproveUsers(_usrList);
				ViewBag.Result = "User Approved Successfully";
			}
			else
			{
				ViewBag.Result = "You are not Authorized.";
			}
			return Json(new { Result = true });
		}

		[HttpPost]
		public ActionResult DeleteSelected(ICollection<int> selectedIds)
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			if (selectedIds != null)
			{
				_userService.DeleteUsers(_userService.GetUsersByIds(selectedIds.ToArray()).ToList());
			}

			return Json(new { Result = true });
		}

		[HttpPost]
		public ActionResult ToggleUser(string id)
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			if (String.IsNullOrEmpty(id))
				throw new Exception("Id Not Found");

			var _user = _userService.GetUserById(Convert.ToInt32(id));

			if (_user != null)
				_userService.ToggleUser(Convert.ToInt32(id));

			if (_user.IsActive)
			{
				SuccessNotification("User activated successfully.");
			}
			else
			{
				SuccessNotification("User de-activated successfully.");
			}
			return View("List");
		}

		public ActionResult Settings()
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			var model = new UserSettingsModel();
			model.ActiveSettings = "UserSettings";

			var userSettings = _settingService.GetSettingsByType(SettingTypeEnum.UserSetting).ToList();
			if (userSettings.Count > 0)
			{

			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Settings(UserSettingsModel model)
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			if (ModelState.IsValid)
			{
				var userSettings = _settingService.GetSettingsByType(SettingTypeEnum.UserSetting).ToList();
				if (userSettings.Count > 0)
				{

				}
			}

			SuccessNotification("User settings saved successfully.");
			model.ActiveSettings = "UserSettings";
			return View(model);
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageUsers"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			if (id != 1)
				_userService.Delete(id);

			SuccessNotification("User deleted successfully.");
			return RedirectToAction("List");
		}

		#endregion

		#region Json Actions

		public PartialViewResult GetAllUsers()
		{
			var model = new List<UserModel>();
			model = _userService.GetAllUsers().Where(x => x.IsDeleted == false).ToList().Select(x => new UserModel()
			{
				CreatedOn = x.CreatedOn,
				Id = x.Id,
				IsActive = x.IsActive,
				IsApproved = x.IsApproved,
				ChangePassword = new ChangePasswordModel() { Id = x.Id },
				AvailableRoles = x.Roles.Select(r => new RoleModel()
				{
					Id = r.Id,
					IsActive = r.IsActive,
					IsDeleted = r.IsDeleted,
					RoleName = r.RoleName,
					IsSystemDefined = r.IsSystemDefined
				}).ToList(),
				Username = x.UserName
			}).ToList();

			return PartialView("_AllUsers", model);
		}

		public JsonResult GetOldPassword(int userid)
		{
			var _User = _userService.GetUserById(userid);
			return Json(_User != null ? _User.Password : "", JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			var _User = _userService.GetUserById(model.Id);
			if (_User != null)
			{
				if (_User.Password.Trim().ToLower() != model.OldPassword.Trim().ToLower())
				{
					ErrorNotification("Password does not match, Please enter your current password.");
					return RedirectToAction("Edit", new { @id = model.Id });
				}
				else
				{
					_User.Password = model.Password.Trim();
					_userService.Update(_User);
					SuccessNotification("Password Changed Successfully.");
				}
			}

			return RedirectToAction("List");
		}

		#endregion

	}
}
