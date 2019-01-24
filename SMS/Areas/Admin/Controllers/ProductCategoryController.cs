using System;
using EF.Services.Http;
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
    public class ProductCategoryController : AdminAreaController
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
        private readonly IPictureService _pictureService;

        #endregion Fileds

        #region Constructor

        public ProductCategoryController(IUserService userService, IUserContext userContext, ISettingService settingService, IRoleService roleService, IPermissionService permissionService, ISMSService smsService, ICommentService commentService, IReplyService replyService, IPictureService pictureService)
        {
            this._userService = userService;
            this._userContext = userContext;
            this._settingService = settingService;
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._smsService = smsService;
            this._commentService = commentService;
            this._replyService = replyService;
            this._pictureService = pictureService;
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
                var categoryData = (from tempcategory in _smsService.GetAllProductCategories() select tempcategory);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    categoryData = categoryData.Where(m => m.Name.Contains(searchValue));
                }

                //total number of rows count     
                var lstCategory = categoryData as ProductCategory[] ?? categoryData.ToArray();
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
                        data = data.Select(x => new ProductCategoryModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            IsActive = x.IsActive,
                            Description = x.Description,
                            DisplayOrder = x.DisplayOrder,
                            IncludeInTopMenu = x.IncludeInTopMenu,
                            ParentCategoryId = x.ParentCategoryId,
                            PictureId = x.PictureId,
                            PictureSrc = x.PictureId > 0 ? _pictureService.GetPictureById(x.PictureId).PictureSrc : "",
                            SystemName = x.SystemName,
                            UserId = x.UserId,
                            Url = Url.RouteUrl("ProductCategory", new { name = x.GetSystemName() }, "http")
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

        public JsonResult GetAllProducts(int categoryid)
        {
            var allProductCategories = _smsService.GetAllProductCategories();
            var productsByCategory = _smsService.GetAllProductsByProductCategory(categoryid);

            //Returning Json Data
            return new JsonResult()
            {
                Data = allProductCategories.Select(x => new ProductCategoryModel()
                {
                    IsActive = x.IsActive,
                    UserId = x.UserId,
                    Name = x.Name.Trim(),
                    Id = x.Id,
                    Selected = productsByCategory.Any(y => y.ProductCategories.Any(z => z.ProductCategoryId == x.Id)),
                    DisplayOrder = x.DisplayOrder,
                    Description = x.Description,
                    IncludeInTopMenu = x.IncludeInTopMenu,
                    ParentCategoryId = x.ParentCategoryId,
                    PictureId = x.PictureId,
                    PictureSrc = x.PictureId > 0 ? _pictureService.GetPictureById(x.PictureId).PictureSrc : "",
                    SystemName = x.SystemName,
                    Url = Url.RouteUrl("ProductCategory", new { name = x.GetSystemName() }, "http")
                }).OrderBy(x => x.Name).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        public ActionResult LoadProductGrid(int id)
        {
            try
            {
                var classData = (from associatedproduct in _smsService.GetAllProductsByProductCategory(id) select associatedproduct).OrderByDescending(eve => eve.CreatedOn).ToList();
                return new JsonResult()
                {
                    Data = new
                    {
                        data = classData.Select(x => new ProductModel()
                        {
                            Id = x.Id,
                            IsActive = x.IsActive,
                            UserId = x.UserId,
                            Name = !string.IsNullOrEmpty(x.Name) ? x.Name.Trim() : ""
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
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            var model = new ProductCategoryModel();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var model = new ProductCategoryModel();
            var objCategory = _smsService.GetProductCategoryById(id);
            if (objCategory != null)
            {
                model = objCategory.ToModel();
            }

            var allProductCategories = _smsService.GetAllProductCategories().Where(x => x.Id != id && x.ParentCategoryId != x.Id).OrderBy(x => x.DisplayOrder).ToList();
            model.AvailableProductCategories = allProductCategories.Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = model.ParentCategoryId == x.Id
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ProductCategoryModel model, FormCollection frm, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkCategory = _smsService.CheckProductCategoryExists(model.Name, model.Id);
            if (checkCategory)
                ModelState.AddModelError("Name", "A Category with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objCategory = _smsService.GetProductCategoryById(model.Id);
                if (objCategory != null)
                {
                    objCategory = model.ToEntity(objCategory);
                    objCategory.ModifiedOn = DateTime.Now;
                    _smsService.UpdateProductCategory(objCategory);
                }
            }
            else
            {
                var allProductCategories = _smsService.GetAllProductCategories().Where(x => x.Id != model.Id && x.ParentCategoryId != x.Id).OrderBy(x => x.DisplayOrder).ToList();
                model.AvailableProductCategories = allProductCategories.Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = model.ParentCategoryId == x.Id
                }).ToList();
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
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            var model = new ProductCategoryModel();
            var allProductCategories = _smsService.GetAllProductCategories().OrderBy(x => x.DisplayOrder).ToList();
            model.AvailableProductCategories = allProductCategories.Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString()
            }).ToList();
            return View(model);
        }

        [HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ProductCategoryModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            // Check for duplicate classroom, if any
            var checkCategory = _smsService.CheckProductCategoryExists(model.Name, model.Id);
            if (checkCategory)
                ModelState.AddModelError("Name", "A Category with the same name already exists. Please choose a different name.");

            if (ModelState.IsValid)
            {
                var objCategory = model.ToEntity();
                objCategory.CreatedOn = objCategory.ModifiedOn = DateTime.Now;
                objCategory.UserId = _userContext.CurrentUser.Id;
                _smsService.InsertProductCategory(objCategory);
                SuccessNotification("Category created successfully.");
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = objCategory.Id });
                }
            }
            else
            {
                var allProductCategories = _smsService.GetAllProductCategories().OrderBy(x => x.DisplayOrder).ToList();
                model.AvailableProductCategories = allProductCategories.Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString()
                }).ToList();

                return View(model);
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Id Not Found");

            _smsService.DeleteProductCategory(id);

            SuccessNotification("Category deleted successfully.");
            return RedirectToAction("List");
        }

        public ActionResult Toggle(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleActiveStatusProductCategory(id);
            ViewBag.Result = "Category updated Successfully";

            return Json(new { Result = true });
        }

        public ActionResult ToggleMenuStatus(int id)
        {
            var user = _userContext.CurrentUser;
            if (id == 0)
                throw new ArgumentNullException("id");

            _smsService.ToggleMenuStatusProductCategory(id);
            ViewBag.Result = "Category updated Successfully";

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult UpdateProductsForCategory(int id, int[] products)
        {
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            if (id == 0)
                throw new ArgumentNullException("id");

            var objProductCategory = _smsService.GetProductCategoryById(id);
            if (objProductCategory != null)
            {
                var objProducts = _smsService.GetAllProductsByProductCategory(id);
                if (products != null && products.Length > 0)
                {
                    var displayOrder = 0;
                    foreach (var productid in products)
                    {
                        var checkRecords = _smsService.GetCategoryProductMappings(categoryid: id, productid: productid);
                        if (checkRecords.Count == 0)
                        {
                            displayOrder += 1;
                            _smsService.InsertProductCategoryMapping(new ProductCategoryMapping()
                            {
                                ProductCategoryId = id,
                                ProductId = productid,
                                DisplayOrder = displayOrder,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                UserId = _userContext.CurrentUser.Id
                            });
                        }
                    }
                }
                else
                {
                    foreach (var record in objProducts)
                    {
                        var objMappings = _smsService.GetCategoryProductMappings(categoryid: id, productid: record.Id);
                        if (objMappings != null && objMappings.Count > 0)
                        {
                            foreach (var obj in objMappings)
                            {
                                _smsService.DeleteProductCategoryMapping(obj.Id);
                            }
                        }
                    }
                }
            }
            ViewBag.Result = "Product Category updated Successfully";
            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult RemoveProductFromCategory(int id, int productid)
        {
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            if (id == 0)
                throw new Exception("Product Category id not found");

            _smsService.RemoveProductFromCategory(id, productid);

            SuccessNotification("Product Category removed successfully");
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
