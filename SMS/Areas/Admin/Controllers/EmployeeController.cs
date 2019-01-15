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
using EF.Core.Enums;

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
                        data = data.Select(x => new EmployeeModel() {
                            Id = x.Id,
                            EmpFName = x.EmpFName,
                            EmpMName = !string.IsNullOrEmpty(x.EmpMName) ? x.EmpMName : "",
                            EmpLName = !string.IsNullOrEmpty(x.EmpLName) ? x.EmpLName : "",
                            Designation = _smsService.GetDesignationById(x.DesignationId).Name,
                            JoiningDate = x.JoiningDate.Value,
                            Sex = x.Sex,
                            IsActive = x.IsActive,
                            ContractStartDateString = x.ContractStartDate != null ? x.ContractStartDate.Value.ToString("dd MMMM yyyy") : "",
                            ContractEndDateString = x.ContractEndDate != null ? x.ContractEndDate.Value.ToString("dd MMMM yyyy") : "",
                            PictureSrc = x.EmployeePictureId > 0 ? _pictureService.GetPictureById(x.EmployeePictureId).PictureSrc : "",
                            Username = x.Username
                        })
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
            var model = employee.ToModel();

            model.AvailableContractTypes = (from ContractType d in Enum.GetValues(typeof(ContractType))
                                                select new SelectListItem
                                                {
                                                    Text = EnumExtensions.GetDescriptionByValue<ContractType>(Convert.ToInt32(d)),
                                                    Value = Convert.ToInt32(d).ToString(),
                                                    Selected = (Convert.ToInt32(d) == model.ContractTypeId)
                                                }).ToList();

            model.AvailableContractStatuses = (from ContractStatus d in Enum.GetValues(typeof(ContractStatus))
                                        select new SelectListItem
                                        {
                                            Text = EnumExtensions.GetDescriptionByValue<ContractStatus>(Convert.ToInt32(d)),
                                            Value = Convert.ToInt32(d).ToString(),
                                            Selected = (Convert.ToInt32(d) == model.ContractStatusId)
                                        }).ToList();

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.Id == model.AcadmicYearId
            }).ToList();

            model.AvailableCastes = _smsService.GetAllCastes().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.CasteId == x.Id
            }).OrderBy(x => x.Text).ToList();

            model.AvailableDesignations = _smsService.GetAllDesignations().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.DesignationId == x.Id
            }).OrderBy(x => x.Text).ToList();

            model.AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.QualificationId == x.Id
            }).OrderBy(x => x.Text).ToList();

            model.AvailableReligions = _smsService.GetAllReligions().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.ReligionId == x.Id
            }).OrderBy(x => x.Text).ToList();

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
                model.AvailableContractTypes = (from ContractType d in Enum.GetValues(typeof(ContractType))
                                                select new SelectListItem
                                                {
                                                    Text = EnumExtensions.GetDescriptionByValue<ContractType>(Convert.ToInt32(d)),
                                                    Value = Convert.ToInt32(d).ToString(),
                                                    Selected = (Convert.ToInt32(d) == model.ContractTypeId)
                                                }).ToList();

                model.AvailableContractStatuses = (from ContractStatus d in Enum.GetValues(typeof(ContractStatus))
                                                   select new SelectListItem
                                                   {
                                                       Text = EnumExtensions.GetDescriptionByValue<ContractStatus>(Convert.ToInt32(d)),
                                                       Value = Convert.ToInt32(d).ToString(),
                                                       Selected = (Convert.ToInt32(d) == model.ContractStatusId)
                                                   }).ToList();

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.AcadmicYearId
                }).ToList();

                model.AvailableCastes = _smsService.GetAllCastes().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.CasteId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableDesignations = _smsService.GetAllDesignations().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.DesignationId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.QualificationId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableReligions = _smsService.GetAllReligions().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.ReligionId == x.Id
                }).OrderBy(x => x.Text).ToList();
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
            model.AvailableContractTypes = (from ContractType d in Enum.GetValues(typeof(ContractType))
                                            select new SelectListItem
                                            {
                                                Text = EnumExtensions.GetDescriptionByValue<ContractType>(Convert.ToInt32(d)),
                                                Value = Convert.ToInt32(d).ToString(),
                                                Selected = false
                                            }).ToList();

            model.AvailableContractStatuses = (from ContractStatus d in Enum.GetValues(typeof(ContractStatus))
                                               select new SelectListItem
                                               {
                                                   Text = EnumExtensions.GetDescriptionByValue<ContractStatus>(Convert.ToInt32(d)),
                                                   Value = Convert.ToInt32(d).ToString(),
                                                   Selected = false
                                               }).ToList();

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();

            model.AvailableCastes = _smsService.GetAllCastes().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = false
            }).OrderBy(x => x.Text).ToList();

            model.AvailableDesignations = _smsService.GetAllDesignations().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = false
            }).OrderBy(x => x.Text).ToList();

            model.AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = false
            }).OrderBy(x => x.Text).ToList();

            model.AvailableReligions = _smsService.GetAllReligions().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = false
            }).OrderBy(x => x.Text).ToList();
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(EmployeeModel model)
        {
            if (!_permissionService.Authorize("ManageEmployees"))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var newEmployee = model.ToEntity();
                newEmployee.CreatedOn = newEmployee.ModifiedOn = DateTime.Now;
                newEmployee.IsDeleted = false;
                newEmployee.UserId = _userContext.CurrentUser.Id;
                newEmployee.Username = CodeHelper.GenerateRandomEmployeeUsername();
                _smsService.InsertEmployee(newEmployee);
            }
            else
            {
                model.AvailableContractTypes = (from ContractType d in Enum.GetValues(typeof(ContractType))
                                                select new SelectListItem
                                                {
                                                    Text = EnumExtensions.GetDescriptionByValue<ContractType>(Convert.ToInt32(d)),
                                                    Value = Convert.ToInt32(d).ToString(),
                                                    Selected = (Convert.ToInt32(d) == model.ContractTypeId)
                                                }).ToList();

                model.AvailableContractStatuses = (from ContractStatus d in Enum.GetValues(typeof(ContractStatus))
                                                   select new SelectListItem
                                                   {
                                                       Text = EnumExtensions.GetDescriptionByValue<ContractStatus>(Convert.ToInt32(d)),
                                                       Value = Convert.ToInt32(d).ToString(),
                                                       Selected = (Convert.ToInt32(d) == model.ContractStatusId)
                                                   }).ToList();

                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.AcadmicYearId
                }).ToList();

                model.AvailableCastes = _smsService.GetAllCastes().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.CasteId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableDesignations = _smsService.GetAllDesignations().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.DesignationId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableQualifications = _smsService.GetAllQualifications().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.QualificationId == x.Id
                }).OrderBy(x => x.Text).ToList();

                model.AvailableReligions = _smsService.GetAllReligions().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.ReligionId == x.Id
                }).OrderBy(x => x.Text).ToList();
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
