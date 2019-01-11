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
    public class BookIssueIssueController : AdminAreaController
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

        public BookIssueIssueController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                    bookIssueData = bookIssueData.Where(m => m.Student.FName.Contains(searchValue) || m.Student.MName.Contains(searchValue) || m.Student.LName.Contains(searchValue) || m.Book.Name.Contains(searchValue) || m.Book.Author.Contains(searchValue));
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
                            Student = _smsService.GetStudentById(x.StudentId).FName
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
            if (!_permissionService.Authorize("ManageBookIssue"))
                return AccessDeniedView();

            var model = new BookIssueModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageBookIssue"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new BookIssueModel();
            var objBookIssue = _smsService.GetBookIssueById(id);
            if (objBookIssue != null)
            {
                model = objBookIssue.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.AcadmicYearId == x.Id
            }).ToList();

            model.AvailableBookIssueStatuses = (from BookIssueStatus d in Enum.GetValues(typeof(BookIssueStatus))
                                        select new SelectListItem
                                        {
                                            Text = d.ToString(),
                                            Value = Convert.ToInt32(d).ToString(),
                                            Selected = (Convert.ToInt32(d) == model.BookIssueStatusId)
                                        }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BookIssueModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageBookIssue"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkBookIssue = _smsService.CheckBookIssueExists(model.Name, model.Id);
            if (checkBookIssue)
                ModelState.AddModelError("Name", "A BookIssue with the same name already exists. Please choose a different name.");

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
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.AcadmicYearId == x.Id
                }).ToList();

                model.AvailableBookIssueStatuses = (from BookIssueStatus d in Enum.GetValues(typeof(BookIssueStatus))
                                               select new SelectListItem
                                               {
                                                   Text = d.ToString(),
                                                   Value = Convert.ToInt32(d).ToString(),
                                                   Selected = (Convert.ToInt32(d) == model.BookIssueStatusId)
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
            if (!_permissionService.Authorize("ManageBookIssue"))
                return AccessDeniedView();

            var model = new BookIssueModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();

            model.AvailableBookIssueStatuses = (from BookIssueStatus d in Enum.GetValues(typeof(BookIssueStatus))
                                           select new SelectListItem
                                           {
                                               Text = d.ToString(),
                                               Value = Convert.ToInt32(d).ToString(),
                                           }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(BookIssueModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageBookIssue"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkBookIssue = _smsService.CheckBookIssueExists(model.Name, model.Id);
            if (checkBookIssue)
                ModelState.AddModelError("Name", "A BookIssue with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objBookIssue = model.ToEntity();
                objBookIssue.CreatedOn = objBookIssue.ModifiedOn = DateTime.Now;
                objBookIssue.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertBookIssue(objBookIssue);
                SuccessNotification("BookIssue created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objBookIssue.Id });
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

                model.AvailableBookIssueStatuses = (from BookIssueStatus d in Enum.GetValues(typeof(BookIssueStatus))
                                               select new SelectListItem
                                               {
                                                   Text = d.ToString(),
                                                   Value = Convert.ToInt32(d).ToString(),
                                                   Selected = (Convert.ToInt32(d) == model.BookIssueStatusId)
                                               }).ToList();
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageBookIssue"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _roleService.Delete(id);

            SuccessNotification("BookIssue deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusBookIssue(id);
            ViewBag.Result = "BookIssue updated Successfully";

            return Json(new { Result = true });
        }

    }
}
