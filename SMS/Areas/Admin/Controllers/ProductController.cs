using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Service;
using SMS.Areas.Admin.Models;
using EF.Services.Http;
using SMS.Models;
using SMS.Mappers;

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

		#endregion Fileds

		#region Constructor

		public ProductController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IProductService productService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IFileService fileService, IPermissionService permissionService, ITemplateService templateService, IEmailService emailService, IUrlService urlService)
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
		}

		#endregion

		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			var model = new List<ProductModel>();
			var user = _userContext.CurrentUser;
			var lstProduct = _productService.GetAllProduct().Where(x => x.UserId == user.Id).OrderByDescending(x => x.CreatedOn).ToList();
			if (lstProduct.Count > 0)
			{
				foreach (var record in lstProduct)
				{
					var productModel = new ProductModel();
					productModel.Name = record.Name;
					productModel.UserId = record.UserId;
					productModel.Id = record.Id;
					productModel.Description = record.Description;

					foreach (var p in productModel.Pictures)
					{
						var proPicture = new ProductPictureModel();
						proPicture.Id = p.Id;
						proPicture.PictureId = p.PictureId;
						proPicture.DisplayOrder = p.DisplayOrder;
						proPicture.PicEndDate = p.PicEndDate;
						proPicture.IsDefault = p.IsDefault;
						proPicture.PicStartDate = p.PicStartDate;

						var picture = _pictureService.GetPictureById(p.PictureId);
						if (picture != null)
						{
							proPicture.Picture = new SMS.Models.PictureModel()
							{
								AlternateText = picture.AlternateText,
								CreatedOn = picture.CreatedOn,
								Id = picture.Id,
								ModifiedOn = picture.ModifiedOn,
								Src = picture.PictureSrc,
								Url = picture.Url,
								UserId = picture.UserId
							};
						}
						productModel.Pictures.Add(proPicture);
					}
				};
			}
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

			var record = _productService.GetProductById(id);
			if (record != null)
			{
				model = new ProductModel();
				model.Name = record.Name;
				model.UserId = record.UserId;
				model.Id = record.Id;
				model.Description = record.Description;

				foreach (var p in record.Pictures)
				{
					var proPicture = new ProductPictureModel();
					proPicture.Id = p.Id;
					proPicture.PictureId = p.PictureId;
					proPicture.DisplayOrder = p.DisplayOrder;
					proPicture.PicEndDate = p.EndDate;
					proPicture.IsDefault = p.IsDefault;
					proPicture.PicStartDate = p.StartDate;

					var picture = _pictureService.GetPictureById(p.PictureId);
					if (picture != null)
					{
						proPicture.Picture = new SMS.Models.PictureModel()
						{
							AlternateText = picture.AlternateText,
							CreatedOn = picture.CreatedOn,
							Id = picture.Id,
							ModifiedOn = picture.ModifiedOn,
							Src = picture.PictureSrc,
							Url = picture.Url,
							UserId = picture.UserId
						};
					}
					model.Pictures.Add(proPicture);
				};
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(ProductModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			// Check for duplicate products, if any
			var product = _productService.GetProductByName(model.Name);
			if (product != null && product.Id != model.Id)
				ModelState.AddModelError("Title", "A Product with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var pro = _productService.GetProductById(model.Id);
				if (pro == null || pro.IsDeleted)
					return RedirectToAction("List");

				model.UserId = user.Id;
				pro = model.ToEntity();
				pro.CreatedOn = DateTime.Now;
				pro.ModifiedOn = DateTime.Now;
				_productService.Update(pro);

				// Save URL Record
				model.SystemName = pro.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(pro, model.SystemName);

			}

			SuccessNotification("Product updated successfully.");
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			var model = new ProductModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(ProductModel model, FormCollection frm)
		{
			if (!_permissionService.Authorize("ManageProducts"))
				return AccessDeniedView();

			var currentUser = _userContext.CurrentUser;
			// Check for duplicate products, if any
			var product = _productService.GetProductByName(model.Name);
			if (product != null)
				ModelState.AddModelError("Title", "A Product with the same name already exists. Please choose a different name.");

			model.UserId = currentUser.Id;
			if (ModelState.IsValid)
			{
				var newProduct = new Product()
				{
					CreatedOn = DateTime.Now,
					ModifiedOn = DateTime.Now,
					Name = model.Name,
					UserId = model.UserId,
					Description = model.Description
				};

				_productService.Insert(newProduct);

				// Save URL Record
				model.SystemName = newProduct.ValidateSystemName(model.SystemName, model.Name, true);
				_urlService.SaveSlug(newProduct, model.SystemName);

				// Get Email Settings for Use
				var settings = _settingService.GetSettingsByType(SettingTypeEnum.EmailSetting);

				// Send Notification To The Admin
				if (settings.Count > 0)
				{
					var productaddedtemplate = _settingService.GetSettingByKey("ProductAdded");
					var Template = _templateService.GetTemplateByName(productaddedtemplate.Value);
					if (Template != null)
					{
						var tokens = new List<DataToken>();
						_templateService.AddProductTokens(tokens, newProduct);

						foreach (var dt in tokens)
						{
							Template.BodyHtml = EF.Core.CodeHelper.Replace(Template.BodyHtml.ToString(), "[" + dt.Name + "]", dt.Value, StringComparison.InvariantCulture);
						}

						var setting = _settingService.GetSettingByKey("FromEmail");
						if (!String.IsNullOrEmpty(setting.Value))
							_emailService.SendMailUsingTemplate(setting.Value, newProduct.Name + "- Just Added", Template);
					}
				}

				SuccessNotification("Product created successfully.");
				return RedirectToAction("List");
			}
			else
			{
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

	}
}
