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
    public class BookController : AdminAreaController
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

        public BookController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var bookData = (from tempbook in _smsService.GetAllBooks() select tempbook);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    bookData = bookData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstBook = bookData as Book[] ?? bookData.ToArray();
                recordsTotal = lstBook.Count();
                //Paging     
                var data = lstBook.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new BookModel() {
                            AcadmicYearId = x.AcadmicYearId,
                            Id = x.Id,
                            Name = x.Name,
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            Author = x.Author,
                            BookStatusId = x.BookStatusId,
                            Description = x.Description,
                            Price = x.Price,
                            AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId).Name,
                            BookStatus = x.BookStatusId > 0 ? Enum.GetValues(typeof(BookStatus)).GetValue(x.BookStatusId).ToString() : "",
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
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            var model = new BookModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new BookModel();
            var objBook = _smsService.GetBookById(id);
            if (objBook != null)
            {
                model = objBook.ToModel();
            }

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.AcadmicYearId == x.Id
            }).ToList();

            model.AvailableBookStatuses = (from BookStatus d in Enum.GetValues(typeof(BookStatus))
                                        select new SelectListItem
                                        {
                                            Text = d.ToString(),
                                            Value = Convert.ToInt32(d).ToString(),
                                            Selected = (Convert.ToInt32(d) == model.BookStatusId)
                                        }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BookModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkBook = _smsService.CheckBookExists(model.Name, model.Id);
            if (checkBook)
                ModelState.AddModelError("Name", "A Book with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objBook = _smsService.GetBookById(model.Id);
                if (objBook != null)
                {
                    model.CreatedOn = objBook.CreatedOn;
                    objBook = model.ToEntity(objBook);
                    objBook.ModifiedOn = DateTime.Now;
                    _smsService.UpdateBook(objBook);
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

                model.AvailableBookStatuses = (from BookStatus d in Enum.GetValues(typeof(BookStatus))
                                               select new SelectListItem
                                               {
                                                   Text = d.ToString(),
                                                   Value = Convert.ToInt32(d).ToString(),
                                                   Selected = (Convert.ToInt32(d) == model.BookStatusId)
                                               }).ToList();
                return View(model);
            }

            SuccessNotification("Book updated successfully.");
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

            var model = new BookModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();

            model.AvailableBookStatuses = (from BookStatus d in Enum.GetValues(typeof(BookStatus))
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
        public ActionResult Create(BookModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageBook"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkBook = _smsService.CheckBookExists(model.Name, model.Id);
            if (checkBook)
                ModelState.AddModelError("Name", "A Book with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objBook = model.ToEntity();
                objBook.CreatedOn = objBook.ModifiedOn = DateTime.Now;
                objBook.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertBook(objBook);
                SuccessNotification("Book created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objBook.Id });
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

                model.AvailableBookStatuses = (from BookStatus d in Enum.GetValues(typeof(BookStatus))
                                               select new SelectListItem
                                               {
                                                   Text = d.ToString(),
                                                   Value = Convert.ToInt32(d).ToString(),
                                                   Selected = (Convert.ToInt32(d) == model.BookStatusId)
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

            SuccessNotification("Book deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusBook(id);
            ViewBag.Result = "Book updated Successfully";

            return Json(new { Result = true });
        }

    }
}
