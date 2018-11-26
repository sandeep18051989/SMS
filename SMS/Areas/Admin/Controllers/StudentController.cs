﻿using System;
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
	public class StudentController : AdminAreaController
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

        public StudentController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, IAuditService auditService, ISMSService smsService, IFileService fileService)
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
				var studentData = (from tempstudents in _smsService.GetAllStudents(true) select tempstudents);

				//Search    
				if (!string.IsNullOrEmpty(searchValue))
				{
					studentData = studentData.Where(m => m.FName.Contains(searchValue) || m.MName.Contains(searchValue) || m.LName.Contains(searchValue) || m.FatherFName.Contains(searchValue) || m.FatherLName.Contains(searchValue) || m.FatherMName.Contains(searchValue));
				}

				//total number of rows count     
				recordsTotal = studentData.Count();
				//Paging     
				var data = studentData.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new StudentDataTable()
                        {
                            Id = x.Id,
                            Name = x.FName + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : ""),
                            FatherName = x.FatherFName + (!string.IsNullOrEmpty(x.FatherMName) ? (" " + x.FatherMName) : "") + (!string.IsNullOrEmpty(x.FatherLName) ? (" " + x.FatherLName) : ""),
                            AdmissionDate = x.AdmissionDate.Value.ToString("dd MMM yyyy"),
                            BusFacility = x.BusFacility,
                            DateOfBirth = x.DateOfBirth.Value.ToString("dd MMM yyyy"),
                            EmailAddress = !string.IsNullOrEmpty(x.EmailAddress) ? x.EmailAddress : "",
                            FatherContact = !string.IsNullOrEmpty(x.Father_Contact) ? x.Father_Contact : "",
                            IsActive = x.IsActive,
                            PictureSrc = x.StudentPictureId > 0 ? _pictureService.GetPictureById(x.StudentPictureId)?.PictureSrc : "",
                            Sex = x.Sex,
                            Url = Url.RouteUrl("Student", new { name = x.GetSystemName() }, "http"),
                            Username = !string.IsNullOrEmpty(x.UserName) ? x.UserName.Trim() : ""
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

        [HttpPost]
        public ActionResult LoadFileGrid(int id)
        {
            try
            {
                var fileData = (from associatedfile in _fileService.GetAllFilesByStudent(id) select associatedfile).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = fileData.Select(x => new FileListModel()
                        {
                            Id = x.Id,
                            Title = !string.IsNullOrEmpty(x.Title) ? x.Title.Trim() : "",
                            Type = !string.IsNullOrEmpty(x.Type) ? x.Type.Trim() : "",
                            StudentId = id,
                            FileSrc = !string.IsNullOrEmpty(x.Src) ? x.Src.Trim() : "",
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
			if (!_permissionService.Authorize("ManageStudents"))
				return AccessDeniedView();

			var model = new StudentListModel();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageStudents"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Student Id Missing");

			var student = _smsService.GetStudentById(id);
			var model = new StudentModel()
			{
				Id = student.Id,
				UserId = student.UserId,
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(StudentModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageStudents"))
				return AccessDeniedView();

			// Get Active Acadmic Year
			var acadmicyear = _smsService.GetActiveAcadmicYear();

			// Check for duplicate student, if any
			var allActiveStudents = _smsService.SearchStudents(true, 0, 0, 0);

			if (allActiveStudents.Any(u => u.FName.Trim().ToLower() == model.FName.Trim().ToLower() && u.Id != model.Id))
				ModelState.AddModelError("Name", "A Student with the same name already exists. Please choose a different name.");

			var student = _smsService.GetStudentById(model.Id);

			if (ModelState.IsValid)
			{
				student.Id = model.Id;
				student.UserId = _userContext.CurrentUser.Id;
				_smsService.UpdateStudent(student);
			}
			else
			{
				return View(model);
			}

			SuccessNotification("Student updated successfully.");
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageStudents"))
				return AccessDeniedView();

			var model = new StudentModel();
			return View(model);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(StudentModel model)
		{
			if (!_permissionService.Authorize("ManageStudents"))
				return AccessDeniedView();

			// Check for duplicate student, if any
			var student = _smsService.GetStudentsByName(model.FName, null);

			if (student != null)
				ModelState.AddModelError("Name", "A Student with same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var newStudent = new Student()
				{
					CreatedOn = DateTime.Now,
					ModifiedOn = DateTime.Now,
					IsDeleted = false,
					UserId = _userContext.CurrentUser.Id
				};
				_smsService.InsertStudent(newStudent);
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
			if (!_permissionService.Authorize("ManageStudents"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			if (id != 1)
				_userService.Delete(id);

			SuccessNotification("Student deleted successfully.");
			return RedirectToAction("List");
		}

        #region Student Files

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult StudentFileAddUpdate(int studentId,int fileId, string title, string type)
        {
            if (!_permissionService.Authorize("ManageStudents"))
                return AccessDeniedView();

            if (fileId == 0)
                throw new ArgumentException();

            var thisstudent = _smsService.GetStudentById(studentId);
            if (thisstudent == null)
                throw new ArgumentException("No student found with the specified id");

            var file = _fileService.GetFileById(fileId);
            if (file == null)
                throw new ArgumentException("No file found with the specified id");

            if(!string.IsNullOrEmpty(title) && !thisstudent.Files.Any(x => x.Title.Trim().ToLower() == title.Trim().ToLower()))
            {
                file.Title = title;
                file.Type = type;
                thisstudent.Files.Add(file);
                _smsService.UpdateStudent(thisstudent);
            }
            else if (fileId > 0 && !thisstudent.Files.Any(x => x.Id == fileId))
            {
                file.Title = title;
                file.Type = type;
                thisstudent.Files.Add(file);
                _smsService.UpdateStudent(thisstudent);
            }

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteStudentFile(int id)
        {
            if (!_permissionService.Authorize("ManageStudents"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("File id not found");

            var fileRecord = _fileService.GetFileById(id);
            if (fileRecord != null)
            {
                _fileService.Delete(fileRecord.Id);
            }

            SuccessNotification("Student file deleted successfully");
            return new JsonResult()
            {
                Data = true,
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion

        #endregion

    }
}