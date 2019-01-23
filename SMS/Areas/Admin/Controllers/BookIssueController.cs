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
    public class BookIssueController : AdminAreaController
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

        public BookIssueController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var bookIssueData = (from tempbookIssue in _smsService.GetAllBookIssueIssues() select tempbookIssue);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    bookIssueData = bookIssueData.Where(m => m.Student.FName.Contains(searchValue) || (!string.IsNullOrEmpty(m.Student.MName) && m.Student.MName.Contains(searchValue)) || (!string.IsNullOrEmpty(m.Student.LName) && m.Student.LName.Contains(searchValue)) || m.Book.Name.Contains(searchValue) || (!string.IsNullOrEmpty(m.Book.Author) && m.Book.Author.Contains(searchValue)));
                }

                //total number of rows count     
                var lstBookIssue = bookIssueData as BookIssue[] ?? bookIssueData.ToArray();
                recordsTotal = lstBookIssue.Count();
                //Paging     
                var data = lstBookIssue.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new BookIssueModel() {
                            Id = x.Id,
                            BookId = x.BookId,
                            StudentId = x.StudentId,
                            LibrarianId = x.LibrarianId,
                            UserId = x.UserId,
                            Book = _smsService.GetBookById(x.BookId).Name,
                            EndDate = x.EndDate,
                            Librarian = _smsService.GetEmployeeById(x.LibrarianId).EmpFName,
                            PenaltyAmount = x.PenaltyAmount,
                            StartDate = x.StartDate,
                            Student = _smsService.GetStudentById(x.StudentId).FName,
                            StringStartDate = x.StartDate.Value.ToString("U"),
                            StringEndDate = x.EndDate.Value.ToString("U")
                        }).OrderByDescending(x => x.CreatedOn).ToList()
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
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            var model = new BookIssueModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new BookIssueModel();
            var objBookIssue = _smsService.GetBookIssueById(id);
            if (objBookIssue != null)
            {
                model = objBookIssue.ToModel();
            }

            model.AvailableBooks = _smsService.GetAllBooks().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.BookId == x.Id
            }).ToList();

            model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem()
            {
                Text = x.EmpFName.Trim() + (!string.IsNullOrEmpty(x.EmpMName) ? (" " + x.EmpMName) : "") + (!string.IsNullOrEmpty(x.EmpLName) ? (" " + x.EmpLName) : "") + ("(" + x.Username + ")"),
                Value = x.Id.ToString(),
                Selected = model.LibrarianId == x.Id
            }).ToList();

            model.AvailableStudents = _smsService.GetAllStudents().Select(x => new SelectListItem()
            {
                Text = x.FName.Trim() + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : "") + ("(" + x.UserName + ")"),
                Value = x.Id.ToString(),
                Selected = model.StudentId == x.Id
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BookIssueModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            var checkBookIssue = false;
            if(model.StudentId > 0 && model.BookId > 0)
            {
                // Check for duplicate classroom, if any
                checkBookIssue = _smsService.CheckBookIssueExists(model.StudentId, model.BookId, model.Id);
                if (checkBookIssue)
                    ModelState.AddModelError("BookId", "Book already issued to student!");
            }

            if (ModelState.IsValid)
            {
                var objBookIssue = _smsService.GetBookIssueById(model.Id);
                if (objBookIssue != null)
                {
                    model.CreatedOn = objBookIssue.CreatedOn;
                    objBookIssue = model.ToEntity(objBookIssue);
                    objBookIssue.ModifiedOn = DateTime.Now;
                    _smsService.UpdateBookIssue(objBookIssue);
                }
            }
            else
            {
                model.AvailableBooks = _smsService.GetAllBooks().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.BookId == x.Id
                }).ToList();

                model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem()
                {
                    Text = x.EmpFName.Trim() + (!string.IsNullOrEmpty(x.EmpMName) ? (" " + x.EmpMName) : "") + (!string.IsNullOrEmpty(x.EmpLName) ? (" " + x.EmpLName) : "") + ("(" + x.Username + ")"),
                    Value = x.Id.ToString(),
                    Selected = model.LibrarianId == x.Id
                }).ToList();

                model.AvailableStudents = _smsService.GetAllStudents().Select(x => new SelectListItem()
                {
                    Text = x.FName.Trim() + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : "") + ("(" + x.UserName + ")"),
                    Value = x.Id.ToString(),
                    Selected = model.StudentId == x.Id
                }).ToList();
                return View(model);
            }

            SuccessNotification("BookIssue updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            var model = new BookIssueModel();
            model.AvailableBooks = _smsService.GetAllBooks().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
            }).ToList();

            model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem()
            {
                Text = x.EmpFName.Trim() + (!string.IsNullOrEmpty(x.EmpMName) ? (" " + x.EmpMName) : "") + (!string.IsNullOrEmpty(x.EmpLName) ? (" " + x.EmpLName) : "") + ("(" + x.Username + ")"),
                Value = x.Id.ToString(),
            }).ToList();

            model.AvailableStudents = _smsService.GetAllStudents().Select(x => new SelectListItem()
            {
                Text = x.FName.Trim() + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : "") + ("(" + x.UserName + ")"),
                Value = x.Id.ToString(),
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(BookIssueModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            var checkBookIssue = false;
            if (model.StudentId > 0 && model.BookId > 0)
            {
                // Check for duplicate classroom, if any
                checkBookIssue = _smsService.CheckBookIssueExists(model.StudentId, model.BookId, model.Id);
                if (checkBookIssue)
                    ModelState.AddModelError("BookId", "Book already issued to student.");
            }

            if (ModelState.IsValid)
            {
                var objBookIssue = model.ToEntity();
                objBookIssue.CreatedOn = objBookIssue.ModifiedOn = DateTime.Now;
                objBookIssue.UserId = _userContext.CurrentUser.Id;
                objBookIssue.IssueDate = DateTime.Now;
                _smsService.InsertBookIssue(objBookIssue);
                SuccessNotification("Book issue record created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objBookIssue.Id });
                }
            }
            else
            {
                model.AvailableBooks = _smsService.GetAllBooks().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.BookId > 0 ? model.BookId == x.Id : false
                }).ToList();

                model.AvailableEmployees = _smsService.GetAllEmployees().Select(x => new SelectListItem()
                {
                    Text = x.EmpFName.Trim() + (!string.IsNullOrEmpty(x.EmpMName) ? (" " + x.EmpMName) : "") + (!string.IsNullOrEmpty(x.EmpLName) ? (" " + x.EmpLName) : "") + ("(" + x.Username + ")"),
                    Value = x.Id.ToString(),
                    Selected = model.LibrarianId > 0 ? model.LibrarianId == x.Id : false
                }).ToList();

                model.AvailableStudents = _smsService.GetAllStudents().Select(x => new SelectListItem()
                {
                    Text = x.FName.Trim() + (!string.IsNullOrEmpty(x.MName) ? (" " + x.MName) : "") + (!string.IsNullOrEmpty(x.LName) ? (" " + x.LName) : "") + ("(" + x.UserName + ")"),
                    Value = x.Id.ToString(),
                    Selected = model.StudentId > 0 ? model.StudentId == x.Id : false
                }).ToList();
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _roleService.Delete(id);

            SuccessNotification("Book issue record deleted successfully.");
            return RedirectToAction("List");
        }
    }
}
