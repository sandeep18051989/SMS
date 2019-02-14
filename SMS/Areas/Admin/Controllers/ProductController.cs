using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using EF.Services.Http;
using SMS.Models;
using SMS.Mappers;
using EF.Services;

namespace SMS.Areas.Admin.Controllers
{
	public class ProductController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IProductService _productService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;
		private readonly IFileService _fileService;
		private readonly IPermissionService _permissionService;
		private readonly ITemplateService _templateService;
		private readonly IEmailService _emailService;
		private readonly IUrlService _urlService;
		private readonly ISMSService _smsService;

		#endregion Fileds

		#region Constructor

		public ProductController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IProductService productService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IFileService fileService, IPermissionService permissionService, ITemplateService templateService, IEmailService emailService, IUrlService urlService, ISMSService smsService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._productService = productService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
			this._fileService = fileService;
			this._permissionService = permissionService;
			this._templateService = templateService;
			this._emailService = emailService;
			this._urlService = urlService;
			this._smsService = smsService;
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
				var productData = (from tempproducts in _productService.GetAllProducts() select tempproducts);

				//Sorting    
				//if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
				//{
				//	productData = productData.AsEnumerable().OrderBy($"{sortColumn} {sortColumnDir}");
				//}
				//Search    
				if (!string.IsNullOrEmpty(searchValue))
				{
					productData = productData.Where(m => m.Name.Contains(searchValue) || m.Description.Contains(searchValue));
				}

				//total number of rows count     
				recordsTotal = productData.Count();
				//Paging     
				var data = productData.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data 
                return new JsonResult()
                {
                    Data = new
                    {
                        draw = draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        data = data.Select(x => new ProductListModel()
                        {
                            Description = !string.IsNullOrEmpty(x.Description) ? x.Description : "",
                            CommentsCount = x.Comments.Count,
                            Id = x.Id,
                            AcadmicYear = x.AcadmicYearId.HasValue ? _smsService.GetAcadmicYearById(x.AcadmicYearId.Value)?.Name : "",
                            Url = Url.RouteUrl("Product", new { name = x.GetSystemName() }, "http"),
                            IsActive = x.IsActive,
                            PicturesCount = x.Pictures.Count,
                            ReactionsCount = x.Reactions.Count,
                            FilesCount = x.Files.Count,
                            Name = x.Name,
                            Price = x.Price.ToString("f2"),
                            VideosCount = x.Videos.Count
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
		public ActionResult LoadPictureGrid(int id)
		{
			try
			{
				var productData = (from associatedpicture in _pictureService.GetProductPictureByProductId(id) select associatedpicture).OrderByDescending(eve => eve.CreatedOn).ToList();
				return new JsonResult()
				{
					Data = new
					{
						data = productData.Select(x => new PictureListModel()
						{
							Id = x.Id,
							DisplayOrder = x.DisplayOrder,
							StartDate = x.StartDate?.ToString("yyyy/MM/dd") ?? "",
							EndDate = x.EndDate?.ToString("yyyy/MM/dd") ?? "",
							IsDefault = x.IsDefault,
							ProductId = id,
							PictureId = x.PictureId,
							PictureSrc = x.Picture?.PictureSrc
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

		public ActionResult LoadVideoGrid(int id)
		{
			try
			{
				var productData = (from associatedvideo in _videoService.GetProductVideosByProductId(id) select associatedvideo);
				return new JsonResult()
				{
					Data = new
					{
						data = productData.Select(x => new VideoListModel()
						{
							Id = x.Id,
							DisplayOrder = x.DisplayOrder,
							StartDate = x.StartDate?.ToString("yyyy/MM/dd") ?? "",
							EndDate = x.EndDate?.ToString("yyyy/MM/dd") ?? "",
							ProductId = id,
							VideoId = x.VideoId,
							VideoSrc = x.Video?.VideoSrc
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

        public JsonResult GetAllProductsByCategpry(int categoryid)
        {
            var allProducts = _productService.GetAllProducts();
            var productsByCategory = _smsService.GetAllProductsByProductCategory(categoryid);

            allProducts = allProducts.Where(x => !productsByCategory.Any(y => y.Id == x.Id)).ToList();
            return new JsonResult()
            {
                Data = allProducts.Select(x => x.ToModel()).OrderBy(x => x.Name).ToList(),
                ContentEncoding = Encoding.Default,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion

        #region Product Methods

        public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			var model = new List<ProductModel>();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			var model = new ProductModel();
			var user = _userContext.CurrentUser;

			if (id == 0)
				throw new Exception("Product Id Missing");

			var product = _productService.GetProductById(id);
			if (product != null)
			{
				model = product.ToModel();
			}

            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();
            return View(model);
		}

		[HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(ProductModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			// Check for duplicate products, if any
			var product = _productService.GetProductByName(model.Name);
			if (product != null && product.Id != model.Id)
				ModelState.AddModelError("Name", "A Product with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var pro = _productService.GetProductById(model.Id);
				if (pro == null || pro.IsDeleted)
					return RedirectToAction("List");

                pro = model.ToEntity(product);
				pro.ModifiedOn = DateTime.Now;
				_productService.Update(pro);

				// Save URL Record
				model.SystemName = pro.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(pro, model.SystemName);

			}
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.IsActive
                }).ToList();
                return View(model);
            }

            SuccessNotification("Product updated successfully.");
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

			var model = new ProductModel();
            model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
            {
                Text = x.Name.Trim(),
                Value = x.Id.ToString(),
                Selected = x.IsActive
            }).ToList();
            return View(model);
		}

		[HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(ProductModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			// Check for duplicate products, if any
			var product = _productService.GetProductByName(model.Name);
			if (product != null)
				ModelState.AddModelError("Name", "A Product with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
                var newProduct = model.ToEntity();
                newProduct.CreatedOn = newProduct.ModifiedOn = DateTime.Now;
                newProduct.UserId = _userContext.CurrentUser.Id;
				_productService.Insert(newProduct);

				// Save URL Record
				model.SystemName = newProduct.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(newProduct, model.SystemName);

				SuccessNotification("Product created successfully.");
				if (continueEditing)
				{
					return RedirectToAction("Edit", new { id = model.Id });
				}
				return RedirectToAction("List");
			}
            else
            {
                model.AvailableAcadmicYears = _smsService.GetAllAcadmicYears().Select(x => new SelectListItem()
                {
                    Text = x.Name.Trim(),
                    Value = x.Id.ToString(),
                    Selected = x.IsActive
                }).ToList();
                return View(model);
            }
        }

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			_productService.Delete(id);

			SuccessNotification("Product deleted successfully.");
			return RedirectToAction("List");
		}

        public ActionResult ToggleActiveStatusProduct(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            _productService.ToggleActiveStatusProduct(id);
            SuccessNotification("Product updated successfully");

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize("ManageProducts"))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _productService.DeleteProducts(_productService.GetProductsByIds(selectedIds.ToArray()).ToList());
            }

            SuccessNotification("Products deleted successfully.");
            return RedirectToAction("List");
        }

        #endregion

        #region Product Picture

        [HttpPost]
		public ActionResult DeleteProductPicture(int id)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Picture id not found");

			var pictureRecord = _pictureService.GetProductPictureByPictureId(id);
			if (pictureRecord != null)
			{
				_pictureService.DeleteProductPicture(pictureRecord.Id);
			}
			else
			{
				var picture = _pictureService.GetPictureById(id);
				if (picture != null)
					_pictureService.Delete(picture.Id);
			}

			SuccessNotification("Product picture deleted successfully");
			return new JsonResult()
			{
				Data = true,
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}

		[HttpPost]
		public ActionResult DeleteProductVideo(int id)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Video id not found");

			var videoRecord = _videoService.GetProductVideoByVideoId(id);
			if (videoRecord != null)
			{
				_videoService.DeleteProductVideo(videoRecord.Id);
			}
			else
			{
				var video = _videoService.GetVideoById(id);
				if (video != null)
					_pictureService.Delete(video.Id);
			}

			SuccessNotification("Product video deleted successfully");
			return new JsonResult()
			{
				Data = true,
				ContentEncoding = Encoding.Default,
				ContentType = "application/json",
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				MaxJsonLength = int.MaxValue
			};
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult ProductPictureAdd(int pictureId, int displayOrder, bool isDefault, int productId, DateTime? startDate, DateTime? endDate)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			if (pictureId == 0)
				throw new ArgumentException();

			var thisproduct = _productService.GetProductById(productId);
			if (thisproduct == null)
				throw new ArgumentException("No product found with the specified id");

			var picture = _pictureService.GetPictureById(pictureId);
			if (picture == null)
				throw new ArgumentException("No picture found with the specified id");

			var productPictureData = new EF.Core.Data.ProductPicture();
			productPictureData.PictureId = pictureId;
			productPictureData.ProductId = productId;
			productPictureData.DisplayOrder = displayOrder;
			productPictureData.IsDefault = isDefault;
			productPictureData.UserId = _userContext.CurrentUser.Id;
			productPictureData.CreatedOn = productPictureData.ModifiedOn = DateTime.Now;

			if (startDate.HasValue)
				productPictureData.StartDate = Convert.ToDateTime(startDate.Value.ToString("yyyy-MM-dd"));

			if (endDate.HasValue)
				productPictureData.EndDate = Convert.ToDateTime(endDate.Value.ToString("yyyy-MM-dd"));

			_pictureService.InsertProductPicture(productPictureData);

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult UpdateProductPicture(int productId, int pictureId, int displayOrder, bool isDefault, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			if (pictureId == 0)
				throw new ArgumentException();

			var thisproduct = _productService.GetProductById(productId);
			if (thisproduct == null)
				throw new ArgumentException("No product found with the specified id");

			var picture = _pictureService.GetPictureById(pictureId);
			if (picture == null)
				throw new ArgumentException("No picture found with the specified id");

			var productPictureData = _pictureService.GetProductPictureByPictureId(pictureId);
			if (productPictureData != null)
			{
				productPictureData.DisplayOrder = displayOrder;
				productPictureData.IsDefault = isDefault;
				productPictureData.ModifiedOn = DateTime.Now;

				if (productPictureData.IsDefault)
				{
					var productDefaultPicture = _pictureService.GetDefaultProductPicture(productId);
					if (productDefaultPicture != null && productDefaultPicture.PictureId != productPictureData.PictureId)
					{
						productDefaultPicture.IsDefault = false;
						_pictureService.UpdateProductPicture(productDefaultPicture);
					}
				}

				DateTime picStartDate = new DateTime(startYear, startMonth, startDay);
				productPictureData.StartDate = picStartDate;

				DateTime picEndDate = new DateTime(endYear, endMonth, endDay);
				productPictureData.EndDate = picEndDate;

				_pictureService.UpdateProductPicture(productPictureData);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult ProductVideoAdd(int videoId, int displayOrder, int productId, DateTime? startDate, DateTime? endDate)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			if (videoId == 0)
				throw new ArgumentException();

			var thisproduct = _productService.GetProductById(productId);
			if (thisproduct == null)
				throw new ArgumentException("No product found with the specified id");

			var video = _videoService.GetVideoById(videoId);
			if (video == null)
				throw new ArgumentException("No video found with the specified id");

			var productVideoData = new EF.Core.Data.ProductVideo();
			productVideoData.VideoId = videoId;
			productVideoData.ProductId = productId;
			productVideoData.DisplayOrder = displayOrder;
			productVideoData.UserId = _userContext.CurrentUser.Id;
			productVideoData.CreatedOn = productVideoData.ModifiedOn = DateTime.Now;

			if (startDate.HasValue)
				productVideoData.StartDate = Convert.ToDateTime(startDate.Value.ToString("yyyy-MM-dd"));

			if (endDate.HasValue)
				productVideoData.EndDate = Convert.ToDateTime(endDate.Value.ToString("yyyy-MM-dd"));

			_videoService.InsertProductVideo(productVideoData);

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetProductPictures(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstProductPictures = new List<ProductPictureModel>();
			var result = _pictureService.GetProductPictureByProductId(id);
			foreach (var x in result)
			{
				var productpic = x.ToModel();
				productpic.PicStartDate = x.StartDate;
				productpic.PicEndDate = x.EndDate;

				if (x.PictureId > 0)
				{
					var picture = _pictureService.GetPictureById(x.PictureId);
					productpic.Picture = new PictureModel()
					{
						AlternateText = picture.AlternateText,
						CreatedOn = picture.CreatedOn,
						Id = picture.Id,
						ModifiedOn = picture.ModifiedOn,
                        PictureSrc = picture.PictureSrc,
						Url = picture.Url,
						UserId = picture.UserId
					};
				}
				lstProductPictures.Add(productpic);
			}
			return Json(lstProductPictures, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetProductVideos(int id)
		{
			if (id == 0)
				throw new ArgumentNullException();

			var lstProductVideos = new List<ProductVideoModel>();
			var result = _videoService.GetProductVideosByProductId(id);
			foreach (var x in result)
			{
				var productpic = x.ToModel();
				productpic.VidStartDate = x.StartDate;
				productpic.VidEndDate = x.EndDate;

				if (x.VideoId > 0)
				{
					var picture = _videoService.GetVideoById(x.VideoId);
					productpic.Video = new VideoModel()
					{
						CreatedOn = picture.CreatedOn,
						Id = picture.Id,
						ModifiedOn = picture.ModifiedOn,
						VideoSrc = picture.VideoSrc,
						Url = picture.Url,
						UserId = picture.UserId
					};
				}
				lstProductVideos.Add(productpic);
			}
			return Json(lstProductVideos, JsonRequestBehavior.AllowGet);
		}

		[ValidateInput(false)]
		[HttpPost]
		public virtual ActionResult UpdateProductVideo(int productId, int videoId, int displayOrder, int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			if (videoId == 0)
				throw new ArgumentException();

			var thisproduct = _productService.GetProductById(productId);
			if (thisproduct == null)
				throw new ArgumentException("No product found with the specified id");

			var video = _videoService.GetVideoById(videoId);
			if (video == null)
				throw new ArgumentException("No video found with the specified id");

			var productVideoData = _videoService.GetProductVideoByVideoId(videoId);
			if (productVideoData != null)
			{
				productVideoData.DisplayOrder = displayOrder;
				productVideoData.ModifiedOn = DateTime.Now;

				DateTime picStartDate = new DateTime(startYear, startMonth, startDay);
				productVideoData.StartDate = picStartDate;

				DateTime picEndDate = new DateTime(endYear, endMonth, endDay);
				productVideoData.EndDate = picEndDate;

				_videoService.UpdateProductVideo(productVideoData);
			}

			return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
		}

		#endregion


	}
}
