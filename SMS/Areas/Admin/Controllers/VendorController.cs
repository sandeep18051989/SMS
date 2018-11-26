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
	public class VendorController : AdminAreaController
	{

		#region Fields

		private readonly IUserService _userService;
		private readonly IPictureService _pictureService;
		private readonly IUserContext _userContext;
		private readonly ISliderService _sliderService;
		private readonly ISettingService _settingService;
		private readonly IVideoService _videoService;
		private readonly ICommentService _commentService;
		private readonly IReplyService _replyService;
		private readonly IBlogService _blogService;
		private readonly IPermissionService _permissionService;
		private readonly IUrlHelper _urlHelper;
		private readonly IUrlService _urlService;
		private readonly ISMSService _smsService;
		private readonly IProductService _productService;

		#endregion Fileds

		#region Constructor

		public VendorController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IPermissionService permissionService, IUrlHelper urlHelper, IUrlService urlService, ISMSService smsService, IProductService productService)
		{
			this._userService = userService;
			this._pictureService = pictureService;
			this._userContext = userContext;
			this._sliderService = sliderService;
			this._settingService = settingService;
			this._videoService = videoService;
			this._commentService = commentService;
			this._replyService = replyService;
			this._blogService = blogService;
			this._permissionService = permissionService;
			this._urlHelper = urlHelper;
			this._urlService = urlService;
			this._smsService = smsService;
			this._productService = productService;
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
				var vendorData = (from tempvendors in _smsService.GetAllVendors() select tempvendors);

				//Search    
				if (!string.IsNullOrEmpty(searchValue))
				{
					vendorData = vendorData.Where(m => m.Name.Contains(searchValue));
				}

				//total number of rows count     
				var enumerable = vendorData as Vendor[] ?? vendorData.ToArray();
				recordsTotal = enumerable.Count();
				//Paging     
				var data = enumerable.Skip(skip).Take(pageSize).ToList();

				//Returning Json Data 
				return new JsonResult()
				{
					Data = new
					{
						draw = draw,
						recordsFiltered = recordsTotal,
						recordsTotal = recordsTotal,
						data = data.Select(x => new VendorModel()
						{
							Id = x.Id,
							Name = x.Name,
							Address = x.Address,
							MobileContact = x.MobileContact,
							OfficeContact = x.OfficeContact,
							ProductCount = _productService.GetProductsByVendor(x.Id).Count
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
		public ActionResult LoadProductsGrid(int id)
		{
			try
			{
				var productData = (from associatedproduct in _productService.GetAllProduct().Where(p => p.VendorId == 0 || p.VendorId == id) select associatedproduct).OrderByDescending(eve => eve.CreatedOn).ToList();
				var lstProduct = new List<ProductModel>();
				foreach (var x in productData)
				{
					var model = new ProductModel();
					model.Id = x.Id;
					model.Name = x.Name;
					model.Description = x.Description;
					model.Selected = x.VendorId == id;
					model.UserId = x.UserId;
					model.CreatedOn = x.CreatedOn;
					model.SystemName = x.GetSystemName();
					model.Url = Url.RouteUrl("Product", new {name = x.GetSystemName()}, "http");

					var defaultPicture = _pictureService.GetDefaultProductPicture(x.Id);
					if (defaultPicture != null)
					{
						model.DefaultPicture.Src = defaultPicture.Picture.PictureSrc;
					}
					lstProduct.Add(model);
				}
				return new JsonResult()
				{
					Data = new
					{
						data = lstProduct
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

		#region Vendor Methods

		public ActionResult List()
		{
			if (!_permissionService.Authorize("ManageVendors"))
				return AccessDeniedView();

			var model = new List<VendorModel>();
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize("ManageVendors"))
				return AccessDeniedView();

			var model = new VendorModel();
			if (id == 0)
				throw new Exception("Vendor Id Missing");

			var eve = _smsService.GetVendorById(id);
			if (eve != null)
			{
				model = eve.ToModel();
			}

			return View(model);
		}

		[HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(VendorModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageVendors"))
				return AccessDeniedView();

			var user = _userContext.CurrentUser;
			// Check for duplicate vendor, if any
			var vendor = _smsService.GetVendorsByName(model.Name);
			if (vendor != null && vendor.Id != model.Id)
				ModelState.AddModelError("Title", "An Vendor with the same name already exists. Please choose a different name.");

			if (ModelState.IsValid)
			{
				var eve = _smsService.GetVendorById(model.Id);

				if (eve == null || eve.IsDeleted)
					return RedirectToAction("List");

				eve.CreatedOn = DateTime.Now;
				eve = model.ToEntity();
				eve.ModifiedOn = DateTime.Now;
				eve.UserId = user.Id;
				_smsService.UpdateVendor(eve);
			}
			else
			{
				ErrorNotification("An error occured while updating vendor. Please try again.");
				return View(model);
			}

			SuccessNotification("Vendor updated successfully.");
			if (continueEditing)
			{
				return RedirectToAction("Edit", new { id = model.Id });
			}
			return RedirectToAction("List");
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize("ManageVendors"))
				return AccessDeniedView();

			var model = new VendorModel();
			return View(model);
		}

		[HttpPost, ParameterOnFormSubmit("save-continue", "continueEditing")]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(VendorModel model, FormCollection frm, bool continueEditing)
		{
			if (!_permissionService.Authorize("ManageVendors"))
				return AccessDeniedView();

			var currentUser = _userContext.CurrentUser;
			// Check for duplicate vendor, if any
			Vendor newVendor;
			var vendor = _smsService.GetVendorsByName(model.Name);
			if (vendor != null)
				ModelState.AddModelError("Title", "An Vendor with the same name already exists. Please choose a different name.");

			model.UserId = currentUser.Id;
			if (ModelState.IsValid)
			{
				model.CreatedOn = model.ModifiedOn = DateTime.Now;
				model.AcadmicYearId = _userContext.CurrentAcadmicYear.Id;
				newVendor = model.ToEntity();

				_smsService.InsertVendor(newVendor);
			}
			else
			{
				ErrorNotification("An error occured while creating vendor. Please try again.");
				return View(model);
			}

			SuccessNotification("Vendor created successfully.");
			if (continueEditing)
			{
				return RedirectToAction("Edit", new { id = newVendor.Id });
			}
			return RedirectToAction("List");
		}

		public ActionResult Delete(int id)
		{
			if (!_permissionService.Authorize("ManageVendors"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Id Not Found");

			_smsService.DeleteVendor(id);

			SuccessNotification("Vendor deleted successfully");
			return RedirectToAction("List");
		}

		#endregion

		#region Vendor Product

		[HttpPost]
		public ActionResult AssignProduct(int id, int productId)
		{
			if (!_permissionService.Authorize("ManageVendors"))
				return AccessDeniedView();

			if (id == 0)
				throw new Exception("Vendor id not found");

			if (productId == 0)
				throw new Exception("Product id not found");

			var productRecord = _productService.GetProductById(productId);
			if (productRecord != null)
			{
				productRecord.VendorId = productRecord.VendorId==id ? 0 : id;
				_productService.Update(productRecord);
			}

			SuccessNotification("Product assigned to vendor successfully");
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
	}
}