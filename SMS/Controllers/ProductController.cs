using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;

namespace SMS.Controllers
{
	public class ProductController : PublicHttpController
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
		private readonly IProductService _productService;
		private readonly ISMSService _smsService;

		#endregion Fileds

		#region Constructor

		public ProductController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IProductService productService, ISMSService smsService)
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
			this._productService = productService;
			this._smsService = smsService;
		}

		#endregion

		// GET: Product
		public ActionResult Index()
		{
			var user = _userContext.CurrentUser;
			var model = new List<ProductModel>();
			var lstProduct = _productService.GetActiveProducts(true);
			if (lstProduct.Count > 0)
			{
				foreach (var record in lstProduct)
				{
					var product = new ProductModel();
					product.Name = record.Name;
					product.Url = record.GetSystemName();
					product.Description = record.Description;
					product.Id = record.Id;
					product.Comments = record.Comments.Select(c => new CommentModel()
					{
						Id = c.Id
					}).ToList();
					product.CreatedOn = record.CreatedOn;
					product.ModifiedOn = record.ModifiedOn;
					product.UserId = record.UserId;

					foreach (var pic in record.Pictures)
					{
						var picture = new ProductPictureModel();
						picture.Id = pic.Id;
						picture.IsDefault = pic.IsDefault;
						picture.DisplayOrder = pic.DisplayOrder;
						picture.PicEndDate = pic.EndDate;
                        picture.PicStartDate = pic.StartDate;
						product.Pictures.Add(picture);

						var productReactions = _smsService.SearchReactions(productid: record.Id);
						if (productReactions.Count > 0)
						{
							foreach (var react in productReactions)
							{
								var reaction = new ReactionModel();
								reaction.ProductId = react.ProductId;
								reaction.IsLike = react.IsLike;
								reaction.IsDislike = react.IsDislike;
								reaction.IsAngry = react.IsAngry;
								reaction.IsHappy = react.IsHappy;
								reaction.IsLol = react.IsLOL;
								reaction.IsSad = react.IsSad;
								reaction.Rating = react.Rating;
								product.Reactions.Add(reaction);
							}
						}
					}

					model.Add(product);
				}
			}

			return PartialView(model);
		}

		public ActionResult List()
		{
			var model = new List<ProductModel>();
			var lstProduct = _productService.GetAllProduct().Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
			if (lstProduct.Count > 0)
			{
				foreach (var record in lstProduct)
				{
					var product = new ProductModel();
					product.Name = record.Name;
					product.Url = record.GetSystemName();
					product.Description = record.Description;
					product.Id = record.Id;
					product.Comments = record.Comments.Select(c => new CommentModel()
					{
						Id = c.Id
					}).ToList();
					product.CreatedOn = record.CreatedOn;
					product.ModifiedOn = record.ModifiedOn;
					product.UserId = record.UserId;

					foreach (var pic in record.Pictures)
					{
						var picture = new ProductPictureModel();
						picture.Id = pic.Id;
						picture.IsDefault = pic.IsDefault;
						picture.DisplayOrder = pic.DisplayOrder;
						picture.PicEndDate = pic.EndDate;
                        picture.PicStartDate = pic.StartDate;
						product.Pictures.Add(picture);

						var productReactions = _smsService.SearchReactions(productid: record.Id);
						if (productReactions.Count > 0)
						{
							foreach (var react in productReactions)
							{
								var reaction = new ReactionModel();
								reaction.ProductId = react.ProductId;
								reaction.IsLike = react.IsLike;
								reaction.IsDislike = react.IsDislike;
								reaction.IsAngry = react.IsAngry;
								reaction.IsHappy = react.IsHappy;
								reaction.IsLol = react.IsLOL;
								reaction.IsSad = react.IsSad;
								reaction.Rating = react.Rating;
								product.Reactions.Add(reaction);
							}
						}
					}

					model.Add(product);
				}
			}

			return View(model);
		}

		public ActionResult Detail(int id)
		{
			var user = _userContext.CurrentUser;
			var model = new ProductModel();
			var record = _productService.GetProductById(id);
			if (record != null)
			{
				var product = new ProductModel();
				product.Name = record.Name;
				product.Url = record.GetSystemName();
				product.Description = record.Description;
				product.Id = record.Id;
				product.Comments = record.Comments.Select(c => new CommentModel()
				{
					Id = c.Id
				}).ToList();
				product.CreatedOn = record.CreatedOn;
				product.ModifiedOn = record.ModifiedOn;
				product.UserId = record.UserId;

				foreach (var pic in record.Pictures)
				{
					var picture = new ProductPictureModel();
					picture.Id = pic.Id;
					picture.IsDefault = pic.IsDefault;
					picture.DisplayOrder = pic.DisplayOrder;
					picture.PicEndDate = pic.EndDate;
                    picture.PicStartDate = pic.StartDate;
					product.Pictures.Add(picture);

					var productReactions = _smsService.SearchReactions(productid: record.Id);
					if (productReactions.Count > 0)
					{
						foreach (var react in productReactions)
						{
							var reaction = new ReactionModel();
							reaction.ProductId = react.ProductId;
							reaction.IsLike = react.IsLike;
							reaction.IsDislike = react.IsDislike;
							reaction.IsAngry = react.IsAngry;
							reaction.IsHappy = react.IsHappy;
							reaction.IsLol = react.IsLOL;
							reaction.IsSad = react.IsSad;
							reaction.Rating = react.Rating;
							product.Reactions.Add(reaction);
						}
					}
				}

				model = product;
			}
			else
			{
				return RedirectToAction("PageNotFound");
			}

			// Attach Child Model Default Values
			model.postCommentModel.Type = model.postCommentModel.postReplyModel.Type = "Product";
			model.postCommentModel.EntityId = id;
			model.postCommentModel.Username = user != null ? user.UserName : "";
			return View(model);
		}

		public ActionResult PostCommentDetail(int id, bool success)
		{
			var user = _userContext.CurrentUser;
			var model = new ProductModel();
			var record = _productService.GetProductById(id);
			if (record != null)
			{
				var product = new ProductModel();
				product.Name = record.Name;
				product.Url = record.GetSystemName();
				product.Description = record.Description;
				product.Id = record.Id;
				product.Comments = record.Comments.Select(c => new CommentModel()
				{
					Id = c.Id
				}).ToList();
				product.CreatedOn = record.CreatedOn;
				product.ModifiedOn = record.ModifiedOn;
				product.UserId = record.UserId;

				foreach (var pic in record.Pictures)
				{
					var picture = new ProductPictureModel();
					picture.Id = pic.Id;
					picture.IsDefault = pic.IsDefault;
					picture.DisplayOrder = pic.DisplayOrder;
					picture.PicEndDate = pic.EndDate;
                    picture.PicStartDate = pic.StartDate;

					product.Pictures.Add(picture);

					var productReactions = _smsService.SearchReactions(productid: record.Id);
					if (productReactions.Count > 0)
					{
						foreach (var react in productReactions)
						{
							var reaction = new ReactionModel();
							reaction.ProductId = react.ProductId;
							reaction.IsLike = react.IsLike;
							reaction.IsDislike = react.IsDislike;
							reaction.IsAngry = react.IsAngry;
							reaction.IsHappy = react.IsHappy;
							reaction.IsLol = react.IsLOL;
							reaction.IsSad = react.IsSad;
							reaction.Rating = react.Rating;
							product.Reactions.Add(reaction);
						}
					}
				}

				model = product;

			}

			// Attach Child Model Default Values
			model.postCommentModel.Type = model.postCommentModel.postReplyModel.Type = "Product";
			model.postCommentModel.EntityId = id;
			model.postCommentModel.Username = user != null ? user.UserName : "";
			model.IsInValidState = success;

			return View("~/Views/Product/Detail.cshtml", model);
		}

		// GET: Product
		public ActionResult FooterProductListColumn1()
		{
			var model = new List<ProductModel>();
			var lstProducts = _productService.GetActiveProducts(true);
			if (lstProducts.Count > 0)
			{
				foreach (var record in lstProducts)
				{
					model.Add(new ProductModel()
					{
						Id = record.Id,
						Name = record.Name,
						Url = record.GetSystemName()
					});
				}
			}
			return PartialView(model);
		}

		// GET: Product
		public ActionResult FooterProductListColumn2()
		{
			var model = new List<ProductModel>();
			var lstProducts = _productService.GetActiveProducts(true);
			if (lstProducts.Count > 0)
			{
				foreach (var record in lstProducts)
				{
					var product = new ProductModel();
					product.Id = record.Id;
					product.Name = record.Name;
					product.Url = record.GetSystemName();
					model.Add(product);
				}
			}
			return PartialView(model);
		}
	}
}
