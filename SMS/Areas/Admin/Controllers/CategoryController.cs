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
    public class CategoryController : AdminAreaController
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

        public CategoryController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService)
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
                var categoryData = (from tempcategory in _smsService.GetAllCategories() select tempcategory);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    categoryData = categoryData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstCategory = categoryData as Category[] ?? categoryData.ToArray();
                recordsTotal = lstCategory.Count();
                //Paging     
                var data = lstCategory.Skip(skip).Take(pageSize);

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new CategoryModel()
                        {
                            CreatedOnString = x.CreatedOn.ToString("U"),
                            ModifiedOnString = x.ModifiedOn.ToString("U"),
                            Id = x.Id,
                            Name = x.Name,
                            IsActive = x.IsActive
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

        public JsonResult GetAllCastes(int categoryid)
        {
            var allCastes = _smsService.GetAllCastes();
            var castesByCategory = _smsService.GetAllCastesByCategory(categoryid);

            //Returning Json Data 
            return new JsonResult()
            {
                Data = allCastes.Select(x => new CasteModel()
                {
                    IsActive = x.IsActive,
                    UserId = x.UserId,
                    Name = x.Name.Trim(),
                    AcadmicYearId = x.AcadmicYearId,
                    AcadmicYear = _smsService.GetAcadmicYearById(x.AcadmicYearId)?.Name,
                    CreatedOnString = x.CreatedOn.ToString("U"),
                    ModifiedOnString = x.ModifiedOn.ToString("U"),
                    Id = x.Id,
                    Selected = castesByCategory.Any(y => y.Id == x.Id)
                }).OrderBy(x => x.Name).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion
        public ActionResult List()
        {
            if (!_permissionService.Authorize("ManageCategory"))
                return AccessDeniedView();

            var model = new CategoryModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageCategory"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new CategoryModel();
            var objCategory = _smsService.GetCategoryById(id);
            if (objCategory != null)
            {
                model = objCategory.ToModel();
            }
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CategoryModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageCategory"))
                return AccessDeniedView();

            var user = _userContext.CurrentUser;
            // Check for duplicate classroom, if any
            var checkCategory = _smsService.CheckCategoryExists(model.Name, model.Id);
            if (checkCategory)
                ModelState.AddModelError("Name", "A Category with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objCategory = _smsService.GetCategoryById(model.Id);
                if (objCategory != null)
                {
                    model.CreatedOn = objCategory.CreatedOn;
                    objCategory = model.ToEntity(objCategory);
                    objCategory.ModifiedOn = DateTime.Now;
                    _smsService.UpdateCategory(objCategory);
                }
            }
            else
            {
                return View(model);
            }

            SuccessNotification("Category updated successfully.");
            if (continueEditing)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize("ManageCategory"))
                return AccessDeniedView();

            var model = new CategoryModel();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(CategoryModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageCategory"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkCategory = _smsService.CheckCategoryExists(model.Name, model.Id);
            if (checkCategory)
                ModelState.AddModelError("Name", "A Category with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objCategory = model.ToEntity();
                objCategory.CreatedOn = objCategory.ModifiedOn = DateTime.Now;
                objCategory.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertCategory(objCategory);
                SuccessNotification("Category created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objCategory.Id });
                }
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageCategory"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteCategory(id);

            SuccessNotification("Category deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusCategory(id);
            ViewBag.Result = "Category updated Successfully";

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult LoadCasteGrid(int id)
        {
            try
            {
                var classData = (from associatedcaste in _smsService.GetAllCastesByCategory(id) select associatedcaste).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = classData.Select(x => new CasteModel()
                        {
                            Id = x.Id,
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            CreatedOnString = x.CreatedOn.ToString("U"),
                            ModifiedOnString = x.ModifiedOn.ToString("U"),
                            Name = !string.IsNullOrEmpty(x.Name) ? x.Name.Trim() : "",
                            Religion = x.Religion != null && !string.IsNullOrEmpty(x.Religion.Name) ? x.Religion.Name : "",
                            ReligionId = x.ReligionId
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
        public ActionResult UpdateCastesForCategory(int id, int[] castes)
        {
            if (!_permissionService.Authorize("ManageCategory"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var user = _userContext.CurrentUser;
            var objCategory = _smsService.GetCategoryById(id);
            if (objCategory != null)
            {
                var objCastes = _smsService.GetAllCastesByCategory(id);
                objCategory.Castes.Clear();
                if (castes != null && castes.Length > 0)
                {
                    foreach (var casteid in castes)
                    {
                        var casteRecord = _smsService.GetCasteById(casteid);
                        if (casteRecord != null)
                        {
                            objCategory.Castes.Add(casteRecord);
                            _smsService.UpdateCategory(objCategory);
                        }
                    }
                }
            }
            ViewBag.Result = "Category updated Successfully";
            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult RemoveCastegoryCast(int id, int casteid)
        {
            if (!_permissionService.Authorize("ManageCategory"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Category id not found");

            _smsService.RemoveCasteFromCategory(id, casteid);

            SuccessNotification("Caste removed successfully from selected category!");
            return new JsonResult()
            {
                Data = true,
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

    }
}
