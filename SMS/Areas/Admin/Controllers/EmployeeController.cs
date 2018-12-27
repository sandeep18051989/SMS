using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Services;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Mappers;
using SMS.Models;

namespace SMS.Areas.Admin.Controllers
{
    public class EmployeeController : AdminAreaController
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
        private readonly ISMSService _smsService;
        private readonly IFileService _fileService;

        #endregion Fileds

        #region Constructor

        public EmployeeController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, IAuditService auditService, ISMSService smsService, IFileService fileService)
        {
            this._userService = userService;
            this._pictureService = pictureService;
            this._userContext = userContext;
            this._sliderService = sliderService;
            this._settingService = settingService;
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._auditService = auditService;
            this._smsService = smsService;
            this._fileService = fileService;
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
                var employeeData = (from tempemployees in _smsService.GetAllEmployees() select tempemployees);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    employeeData = employeeData.Where(m => m.EmpFName.Contains(searchValue) || m.EmpMName.Contains(searchValue) || m.EmpLName.Contains(searchValue) || m.FatherFName.Contains(searchValue) || m.FatherLName.Contains(searchValue) || m.FatherMName.Contains(searchValue));
                }

                //total number of rows count     
                recordsTotal = employeeData.Count();
                //Paging     
                var data = employeeData.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => x.ToModel())
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
            if (!_permissionService.Authorize("ManageEmployees"))
                return AccessDeniedView();

            var model = new EmployeeModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageEmployees"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Employee Id Missing");

            var employee = _smsService.GetEmployeeById(id);
            var model = new EmployeeModel()
            {
                Id = employee.Id,
                UserId = employee.UserId,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(EmployeeModel model, FormCollection frm)
        {
            if (!_permissionService.Authorize("ManageEmployees"))
                return AccessDeniedView();

            // Get Active Acadmic Year
            var acadmicyear = _smsService.GetActiveAcadmicYear();

            // Check for duplicate employee, if any
            var allActiveEmployees = _smsService.GetAllEmployees();

            if (allActiveEmployees.Any(u => u.EmpFName.Trim().ToLower() == model.EmpFName.Trim().ToLower() && u.Id != model.Id))
                ModelState.AddModelError("Name", "A Employee with the same name already exists. Please choose a different name.");

            var employee = _smsService.GetEmployeeById(model.Id);

            if (ModelState.IsValid)
            {
                model.CreatedOn = employee.CreatedOn;
                employee = model.ToEntity(employee);
                employee.ModifiedOn = DateTime.Now;
                _smsService.UpdateEmployee(employee);
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Employee updated successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageEmployees"))
                return AccessDeniedView();

            var model = new EmployeeModel();
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(EmployeeModel model)
        {
            if (!_permissionService.Authorize("ManageEmployees"))
                return AccessDeniedView();

            // Check for duplicate employee, if any
            var employee = _smsService.CheckEmployeeExists(model.EmpFName, null);

            if (employee)
                ModelState.AddModelError("Name", "A Employee with same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var newEmployee = model.ToEntity();
                newEmployee.CreatedOn = DateTime.Now;
                newEmployee.ModifiedOn = DateTime.Now;
                newEmployee.IsDeleted = false;
                newEmployee.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertEmployee(newEmployee);
            }
            else
            {
                return View(model);
            }

            SuccessNotification("User created successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageEmployees"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            if (id != 1)
                _userService.Delete(id);

            SuccessNotification("Employee deleted successfully.");
            return RedirectToAction("List");
        }



        #endregion

    }
}
