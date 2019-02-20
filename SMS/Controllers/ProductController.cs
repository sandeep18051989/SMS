using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EF.Core;
using EF.Services.Http;
using EF.Services.Service;
using SMS.Models;
using SMS.Mappers;
using SMS.Models.Widgets;
using EF.Services;
using System;
using EF.Core.Data;

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
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleService _roleService;

        #endregion Fileds

        #region Constructor

        public ProductController(IUserService userService, IPictureService pictureService, IUserContext userContext, ISliderService sliderService, ISettingService settingService, IVideoService videoService, ICommentService commentService, IReplyService replyService, IBlogService blogService, IProductService productService, ISMSService smsService, IAuthenticationService authenticationService, IRoleService roleService)
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
            this._authenticationService = authenticationService;
            this._roleService = roleService;
        }

        #endregion

        [ChildActionOnly]
        public ActionResult Index()
        {
            var widgetModel = new ProductListWidgetModel();
            var lstProducts = _productService.GetAllProducts(true).OrderByDescending(x => x.MarkAsNew ? x.AvailableStartDate.HasValue ? x.AvailableStartDate.Value.ToString("U") : x.AvailableEndDate.HasValue ? x.AvailableEndDate.Value.ToString("U") : x.ModifiedOn.Value.ToString("U") : x.Name).ToList();
            if (lstProducts.Count > 0)
            {
                foreach (var product in lstProducts)
                {
                    var model = new ProductModel();
                    model.Id = product.Id;
                    model.Description = product.Description;
                    model.CreatedOn = product.CreatedOn;
                    model.AvailableStartDate = product.AvailableStartDate;
                    model.AvailableEndDate = product.AvailableEndDate;
                    model.BasePrice = product.BasePrice;
                    model.DisableBuyButton = product.DisableBuyButton;
                    model.IsUpcoming = product.IsUpcoming;
                    model.MarkAsNew = product.MarkAsNew;
                    model.MarkAsNewStartDate = product.MarkAsNewStartDate;
                    model.MarkAsNewEndDate = product.MarkAsNewEndDate;
                    model.ModifiedOn = product.ModifiedOn;
                    model.Name = product.Name;
                    model.OldPrice = product.OldPrice;
                    model.Price = product.Price;
                    model.ProductCategory = product.ProductCategories.Count > 0 ? product.ProductCategories.Select(x => x.ProductCategory.Name).FirstOrDefault() : "";
                    model.StockQuantity = product.StockQuantity;
                    model.SystemName = product.GetSystemName(true);

                    var productPictures = _pictureService.GetProductPictureByProductId(product.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultPicture = productPictures.Any(x => x.IsDefault);

                    var productVideos = _videoService.GetProductVideosByProductId(product.Id).OrderByDescending(x => x.StartDate).ToList();
                    model.HasDefaultVideo = productVideos.Count > 0;

                    if (productPictures.Count > 0)
                    {
                        model.DefaultPictureSrc = productPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                        model.Pictures = productPictures.Select(x => x.ToModel()).ToList();
                    }

                    if (productVideos.Count > 0)
                    {
                        model.DefaultVideoSrc = productVideos.FirstOrDefault().Video.VideoSrc;
                        model.Videos = productVideos.Select(x => x.ToModel()).ToList();
                    }

                    model.Reactions = _smsService.SearchReactions(productid: product.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                    model.Comments = _commentService.GetCommentsByProduct(product.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                    if (model.Comments.Count > 0)
                    {
                        foreach (var comment in model.Comments)
                        {
                            comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                        }
                    }

                    model.IsAuthenticated = _userContext.CurrentUser != null;
                    widgetModel.Products.Add(model);
                }
            }
            return PartialView(widgetModel);
        }

        public ActionResult List(PagingFilteringModel command)
        {
            var model = new ProductListWidgetModel();
            var itemsPerPageSetting = _settingService.GetSettingByKey("ItemsPerPage");
            if (command.PageSize <= 0) command.PageSize = itemsPerPageSetting != null ? Convert.ToInt32(itemsPerPageSetting.Value) : 25;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            var products = _productService.GetPagedProducts(keyword: command.Keyword, pageIndex: command.PageNumber - 1, pageSize: command.PageSize, onlyActive: true);
            model.PagingFilteringContext.LoadPagedList(products);

            foreach (var product in products)
            {
                var objProduct = new ProductModel();
                objProduct.Id = product.Id;
                objProduct.Description = product.Description;
                objProduct.CreatedOn = product.CreatedOn;
                objProduct.AvailableStartDate = product.AvailableStartDate;
                objProduct.AvailableEndDate = product.AvailableEndDate;
                objProduct.BasePrice = product.BasePrice;
                objProduct.DisableBuyButton = product.DisableBuyButton;
                objProduct.IsUpcoming = product.IsUpcoming;
                objProduct.MarkAsNew = product.MarkAsNew;
                objProduct.MarkAsNewStartDate = product.MarkAsNewStartDate;
                objProduct.MarkAsNewEndDate = product.MarkAsNewEndDate;
                objProduct.ModifiedOn = product.ModifiedOn;
                objProduct.Name = product.Name;
                objProduct.OldPrice = product.OldPrice;
                objProduct.Price = product.Price;
                objProduct.ProductCategory = product.ProductCategories.Count > 0 ? product.ProductCategories.Select(x => x.ProductCategory.Name).FirstOrDefault() : "";
                objProduct.StockQuantity = product.StockQuantity;
                objProduct.SystemName = product.GetSystemName(true);

                var productPictures = _pictureService.GetProductPictureByProductId(product.Id).OrderByDescending(x => x.StartDate).ToList();
                objProduct.HasDefaultPicture = productPictures.Any(x => x.IsDefault);

                var productVideos = _videoService.GetProductVideosByProductId(product.Id).OrderByDescending(x => x.StartDate).ToList();
                objProduct.HasDefaultVideo = productVideos.Count > 0;

                if (productPictures.Count > 0)
                {
                    objProduct.DefaultPictureSrc = productPictures.FirstOrDefault(x => objProduct.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    objProduct.Pictures = productPictures.Select(x => x.ToModel()).ToList();
                }

                if (productVideos.Count > 0)
                {
                    objProduct.DefaultVideoSrc = productVideos.FirstOrDefault().Video.VideoSrc;
                    objProduct.Videos = productVideos.Select(x => x.ToModel()).ToList();
                }

                objProduct.Reactions = _smsService.SearchReactions(productid: product.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                objProduct.Comments = _commentService.GetCommentsByProduct(product.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                if (objProduct.Comments.Count > 0)
                {
                    foreach (var comment in objProduct.Comments)
                    {
                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                    }
                }

                objProduct.IsAuthenticated = _userContext.CurrentUser != null;
                model.Products.Add(objProduct);
            }

            model.ProductCategories = _smsService.GetAllProductCategories(true).Select(x => x.ToModel()).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(PagingFilteringModel command, FormCollection frm)
        {
            var model = new ProductListWidgetModel();
            var itemsPerPageSetting = _settingService.GetSettingByKey("ItemsPerPage");
            if (command.PageSize <= 0) command.PageSize = itemsPerPageSetting != null ? Convert.ToInt32(itemsPerPageSetting.Value) : 25;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            var products = _productService.GetPagedProducts(keyword: command.Keyword, productcategoryid: command.ProductCategoryId, pageIndex: command.PageNumber - 1, pageSize: command.PageSize, onlyActive: true);
            model.PagingFilteringContext.LoadPagedList(products);
            model.PagingFilteringContext.Keyword = command.Keyword;
            model.PagingFilteringContext.ProductCategoryId = command.ProductCategoryId;

            foreach (var product in products)
            {
                var objProduct = new ProductModel();
                objProduct.Id = product.Id;
                objProduct.Description = product.Description;
                objProduct.CreatedOn = product.CreatedOn;
                objProduct.AvailableStartDate = product.AvailableStartDate;
                objProduct.AvailableEndDate = product.AvailableEndDate;
                objProduct.BasePrice = product.BasePrice;
                objProduct.DisableBuyButton = product.DisableBuyButton;
                objProduct.IsUpcoming = product.IsUpcoming;
                objProduct.MarkAsNew = product.MarkAsNew;
                objProduct.MarkAsNewStartDate = product.MarkAsNewStartDate;
                objProduct.MarkAsNewEndDate = product.MarkAsNewEndDate;
                objProduct.ModifiedOn = product.ModifiedOn;
                objProduct.Name = product.Name;
                objProduct.OldPrice = product.OldPrice;
                objProduct.Price = product.Price;
                objProduct.ProductCategory = product.ProductCategories.Count > 0 ? product.ProductCategories.Select(x => x.ProductCategory.Name).FirstOrDefault() : "";
                objProduct.StockQuantity = product.StockQuantity;
                objProduct.SystemName = product.GetSystemName(true);

                var productPictures = _pictureService.GetProductPictureByProductId(product.Id).OrderByDescending(x => x.StartDate).ToList();
                objProduct.HasDefaultPicture = productPictures.Any(x => x.IsDefault);

                var productVideos = _videoService.GetProductVideosByProductId(product.Id).OrderByDescending(x => x.StartDate).ToList();
                objProduct.HasDefaultVideo = productVideos.Count > 0;

                if (productPictures.Count > 0)
                {
                    objProduct.DefaultPictureSrc = productPictures.FirstOrDefault(x => objProduct.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    objProduct.Pictures = productPictures.Select(x => x.ToModel()).ToList();
                }

                if (productVideos.Count > 0)
                {
                    objProduct.DefaultVideoSrc = productVideos.FirstOrDefault().Video.VideoSrc;
                    objProduct.Videos = productVideos.Select(x => x.ToModel()).ToList();
                }

                objProduct.Reactions = _smsService.SearchReactions(productid: product.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                objProduct.Comments = _commentService.GetCommentsByProduct(product.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                if (objProduct.Comments.Count > 0)
                {
                    foreach (var comment in objProduct.Comments)
                    {
                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                    }
                }

                objProduct.IsAuthenticated = _userContext.CurrentUser != null;
                model.Products.Add(objProduct);
            }

            model.ProductCategories = _smsService.GetAllProductCategories(true).Select(x => x.ToModel()).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var user = _userContext.CurrentUser;
            var product = _productService.GetProductById(id);
            if (product != null)
            {
                var model = new ProductModel();
                model.Id = product.Id;
                model.Description = product.Description;
                model.CreatedOn = product.CreatedOn;
                model.AvailableStartDate = product.AvailableStartDate;
                model.AvailableEndDate = product.AvailableEndDate;
                model.BasePrice = product.BasePrice;
                model.DisableBuyButton = product.DisableBuyButton;
                model.IsUpcoming = product.IsUpcoming;
                model.MarkAsNew = product.MarkAsNew;
                model.MarkAsNewStartDate = product.MarkAsNewStartDate;
                model.MarkAsNewEndDate = product.MarkAsNewEndDate;
                model.ModifiedOn = product.ModifiedOn;
                model.Name = product.Name;
                model.OldPrice = product.OldPrice;
                model.Price = product.Price;
                model.ProductCategory = product.ProductCategories.Count > 0 ? product.ProductCategories.Select(x => x.ProductCategory.Name).FirstOrDefault() : "";
                model.StockQuantity = product.StockQuantity;
                model.SystemName = product.GetSystemName(true);

                var productPictures = _pictureService.GetProductPictureByProductId(product.Id).OrderByDescending(x => x.StartDate).ToList();
                model.HasDefaultPicture = productPictures.Any(x => x.IsDefault);

                var productVideos = _videoService.GetProductVideosByProductId(product.Id).OrderByDescending(x => x.StartDate).ToList();
                model.HasDefaultVideo = productVideos.Count > 0;

                if (productPictures.Count > 0)
                {
                    model.DefaultPictureSrc = productPictures.FirstOrDefault(x => model.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                    model.Pictures = productPictures.Select(x => x.ToModel()).ToList();
                }

                if (productVideos.Count > 0)
                {
                    model.DefaultVideoSrc = productVideos.FirstOrDefault().Video.VideoSrc;
                    model.Videos = productVideos.Select(x => x.ToModel()).ToList();
                }

                model.Reactions = _smsService.SearchReactions(productid: product.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                model.Comments = _commentService.GetCommentsByProduct(product.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                if (model.Comments.Count > 0)
                {
                    foreach (var comment in model.Comments)
                    {
                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                    }
                }

                model.IsAuthenticated = _userContext.CurrentUser != null;

                #region Related Products
                if (product.ProductCategories.Count > 0)
                {
                    model.RelatedProducts.Clear();
                    foreach (var cat in product.ProductCategories)
                    {
                        var productsByCategories = _smsService.GetAllProductsByProductCategory(cat.Id);
                        foreach (var catproduct in productsByCategories)
                        {
                            var relatedProduct = new ProductModel();
                            if (!model.RelatedProducts.Any(x => x.Id == catproduct.Id))
                            {
                                relatedProduct.Id = catproduct.Id;
                                relatedProduct.Description = catproduct.Description;
                                relatedProduct.CreatedOn = catproduct.CreatedOn;
                                relatedProduct.AvailableStartDate = catproduct.AvailableStartDate;
                                relatedProduct.AvailableEndDate = catproduct.AvailableEndDate;
                                relatedProduct.BasePrice = catproduct.BasePrice;
                                relatedProduct.DisableBuyButton = catproduct.DisableBuyButton;
                                relatedProduct.IsUpcoming = catproduct.IsUpcoming;
                                relatedProduct.MarkAsNew = catproduct.MarkAsNew;
                                relatedProduct.MarkAsNewStartDate = catproduct.MarkAsNewStartDate;
                                relatedProduct.MarkAsNewEndDate = catproduct.MarkAsNewEndDate;
                                relatedProduct.ModifiedOn = catproduct.ModifiedOn;
                                relatedProduct.Name = catproduct.Name;
                                relatedProduct.OldPrice = catproduct.OldPrice;
                                relatedProduct.Price = catproduct.Price;
                                relatedProduct.ProductCategory = catproduct.ProductCategories.Count > 0 ? catproduct.ProductCategories.Select(x => x.ProductCategory.Name).FirstOrDefault() : "";
                                relatedProduct.StockQuantity = catproduct.StockQuantity;
                                relatedProduct.SystemName = catproduct.GetSystemName(true);

                                productPictures = _pictureService.GetProductPictureByProductId(catproduct.Id).OrderByDescending(x => x.StartDate).ToList();
                                relatedProduct.HasDefaultPicture = productPictures.Any(x => x.IsDefault);

                                productVideos = _videoService.GetProductVideosByProductId(catproduct.Id).OrderByDescending(x => x.StartDate).ToList();
                                relatedProduct.HasDefaultVideo = productVideos.Count > 0;

                                if (productPictures.Count > 0)
                                {
                                    relatedProduct.DefaultPictureSrc = productPictures.FirstOrDefault(x => relatedProduct.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                                    relatedProduct.Pictures = productPictures.Select(x => x.ToModel()).ToList();
                                }

                                if (productVideos.Count > 0)
                                {
                                    relatedProduct.DefaultVideoSrc = productVideos.FirstOrDefault().Video.VideoSrc;
                                    relatedProduct.Videos = productVideos.Select(x => x.ToModel()).ToList();
                                }

                                relatedProduct.Reactions = _smsService.SearchReactions(productid: catproduct.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                                relatedProduct.Comments = _commentService.GetCommentsByProduct(catproduct.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                                if (relatedProduct.Comments.Count > 0)
                                {
                                    foreach (var comment in relatedProduct.Comments)
                                    {
                                        comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                                    }
                                }

                                relatedProduct.IsAuthenticated = _userContext.CurrentUser != null;

                                // Add To Model
                                model.RelatedProducts.Add(relatedProduct);
                            }
                        }

                    }
                }
                #endregion

                #region New Products
                var newProducts = _productService.GetNewProducts(true);
                foreach (var newproduct in newProducts)
                {
                    var newProduct = new ProductModel();
                    if (!model.NewProducts.Any(x => x.Id == newproduct.Id))
                    {
                        newProduct.Id = newproduct.Id;
                        newProduct.Description = newproduct.Description;
                        newProduct.CreatedOn = newproduct.CreatedOn;
                        newProduct.AvailableStartDate = newproduct.AvailableStartDate;
                        newProduct.AvailableEndDate = newproduct.AvailableEndDate;
                        newProduct.BasePrice = newproduct.BasePrice;
                        newProduct.DisableBuyButton = newproduct.DisableBuyButton;
                        newProduct.IsUpcoming = newproduct.IsUpcoming;
                        newProduct.MarkAsNew = newproduct.MarkAsNew;
                        newProduct.MarkAsNewStartDate = newproduct.MarkAsNewStartDate;
                        newProduct.MarkAsNewEndDate = newproduct.MarkAsNewEndDate;
                        newProduct.ModifiedOn = newproduct.ModifiedOn;
                        newProduct.Name = newproduct.Name;
                        newProduct.OldPrice = newproduct.OldPrice;
                        newProduct.Price = newproduct.Price;
                        newProduct.ProductCategory = newproduct.ProductCategories.Count > 0 ? newproduct.ProductCategories.Select(x => x.ProductCategory.Name).FirstOrDefault() : "";
                        newProduct.StockQuantity = newproduct.StockQuantity;
                        newProduct.SystemName = newproduct.GetSystemName(true);

                        productPictures = _pictureService.GetProductPictureByProductId(newproduct.Id).OrderByDescending(x => x.StartDate).ToList();
                        newProduct.HasDefaultPicture = productPictures.Any(x => x.IsDefault);

                        productVideos = _videoService.GetProductVideosByProductId(newproduct.Id).OrderByDescending(x => x.StartDate).ToList();
                        newProduct.HasDefaultVideo = productVideos.Count > 0;

                        if (productPictures.Count > 0)
                        {
                            newProduct.DefaultPictureSrc = productPictures.FirstOrDefault(x => newProduct.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                            newProduct.Pictures = productPictures.Select(x => x.ToModel()).ToList();
                        }

                        if (productVideos.Count > 0)
                        {
                            newProduct.DefaultVideoSrc = productVideos.FirstOrDefault().Video.VideoSrc;
                            newProduct.Videos = productVideos.Select(x => x.ToModel()).ToList();
                        }

                        newProduct.Reactions = _smsService.SearchReactions(productid: newproduct.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                        newProduct.Comments = _commentService.GetCommentsByProduct(newproduct.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                        if (newProduct.Comments.Count > 0)
                        {
                            foreach (var comment in newProduct.Comments)
                            {
                                comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                            }
                        }

                        newProduct.IsAuthenticated = _userContext.CurrentUser != null;

                        // Add To Model
                        model.NewProducts.Add(newProduct);
                    }
                }
                #endregion

                #region Upcoming Products
                var upcomingProducts = _productService.GetUpcomingProducts(true);
                foreach (var upcomingproduct in upcomingProducts)
                {
                    var upcomingProduct = new ProductModel();
                    if (!model.NewProducts.Any(x => x.Id == upcomingproduct.Id))
                    {
                        upcomingProduct.Id = upcomingproduct.Id;
                        upcomingProduct.Description = upcomingproduct.Description;
                        upcomingProduct.CreatedOn = upcomingproduct.CreatedOn;
                        upcomingProduct.AvailableStartDate = upcomingproduct.AvailableStartDate;
                        upcomingProduct.AvailableEndDate = upcomingproduct.AvailableEndDate;
                        upcomingProduct.BasePrice = upcomingproduct.BasePrice;
                        upcomingProduct.DisableBuyButton = upcomingproduct.DisableBuyButton;
                        upcomingProduct.IsUpcoming = upcomingproduct.IsUpcoming;
                        upcomingProduct.MarkAsNew = upcomingproduct.MarkAsNew;
                        upcomingProduct.MarkAsNewStartDate = upcomingproduct.MarkAsNewStartDate;
                        upcomingProduct.MarkAsNewEndDate = upcomingproduct.MarkAsNewEndDate;
                        upcomingProduct.ModifiedOn = upcomingproduct.ModifiedOn;
                        upcomingProduct.Name = upcomingproduct.Name;
                        upcomingProduct.OldPrice = upcomingproduct.OldPrice;
                        upcomingProduct.Price = upcomingproduct.Price;
                        upcomingProduct.ProductCategory = upcomingproduct.ProductCategories.Count > 0 ? upcomingproduct.ProductCategories.Select(x => x.ProductCategory.Name).FirstOrDefault() : "";
                        upcomingProduct.StockQuantity = upcomingproduct.StockQuantity;
                        upcomingProduct.SystemName = upcomingproduct.GetSystemName(true);

                        productPictures = _pictureService.GetProductPictureByProductId(upcomingproduct.Id).OrderByDescending(x => x.StartDate).ToList();
                        upcomingProduct.HasDefaultPicture = productPictures.Any(x => x.IsDefault);

                        productVideos = _videoService.GetProductVideosByProductId(upcomingproduct.Id).OrderByDescending(x => x.StartDate).ToList();
                        upcomingProduct.HasDefaultVideo = productVideos.Count > 0;

                        if (productPictures.Count > 0)
                        {
                            upcomingProduct.DefaultPictureSrc = productPictures.FirstOrDefault(x => upcomingProduct.HasDefaultPicture ? x.IsDefault : true).Picture.PictureSrc;
                            upcomingProduct.Pictures = productPictures.Select(x => x.ToModel()).ToList();
                        }

                        if (productVideos.Count > 0)
                        {
                            upcomingProduct.DefaultVideoSrc = productVideos.FirstOrDefault().Video.VideoSrc;
                            upcomingProduct.Videos = productVideos.Select(x => x.ToModel()).ToList();
                        }

                        upcomingProduct.Reactions = _smsService.SearchReactions(productid: upcomingproduct.Id).Select(x => x.ToModel()).OrderByDescending(x => x.CreatedOn).ToList();
                        upcomingProduct.Comments = _commentService.GetCommentsByProduct(upcomingproduct.Id).OrderByDescending(x => x.CreatedOn).Select(x => x.ToWidgetModel()).ToList();

                        if (upcomingProduct.Comments.Count > 0)
                        {
                            foreach (var comment in upcomingProduct.Comments)
                            {
                                comment.Replies = _replyService.GetAllRepliesByComment(comment.Id).OrderBy(x => x.DisplayOrder).Select(x => x.ToWidgetModel()).ToList();
                            }
                        }

                        upcomingProduct.IsAuthenticated = _userContext.CurrentUser != null;

                        // Add To Model
                        model.NewProducts.Add(upcomingProduct);
                    }
                }
                #endregion

                return View(model);
            }

            return RedirectToAction("List");
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
                product.Comments = record.Comments.Select(c => c.ToWidgetModel()).ToList();
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
            return View("~/Views/Product/Details.cshtml", model);
        }

        // GET: Product
        public ActionResult FooterProductListColumn1()
        {
            var model = new List<ProductModel>();
            var lstProducts = _productService.GetAllProducts(true);
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
            var lstProducts = _productService.GetAllProducts(true);
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

        [HttpPost]
        public ActionResult Reaction(FormCollection frm)
        {
            if (frm.HasKeys())
            {
                var productId = frm.AllKeys.FirstOrDefault(x => x.StartsWith("submitid_"));
                var userId = frm.AllKeys.FirstOrDefault(x => x.StartsWith("submituserid_"));
                var reactionSubmitted = frm.AllKeys.FirstOrDefault(x => x.StartsWith("submitreaction_"));
                var ratingSubmitted = frm.AllKeys.FirstOrDefault(x => x.StartsWith("submitrating_"));

                var selectedProduct = _productService.GetProductById(Convert.ToInt32(frm[productId].ToString()));
                if (frm[userId] != null && !string.IsNullOrEmpty(frm[userId]) && Convert.ToInt32(frm[userId].ToString()) > 0)
                {
                    string textComments = "";
                    var keyComment = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "txtcomment");
                    if (keyComment != null && !string.IsNullOrEmpty(frm[keyComment].ToString()))
                    {
                        textComments = frm[keyComment].ToString();
                    }

                    var user = _userService.GetUserById(Convert.ToInt32(frm[userId].ToString()));
                    var comments = _commentService.GetCommentsByProduct(selectedProduct.Id);
                    comments = comments.OrderBy(x => x.DisplayOrder).ToList();

                    var newComment = new CommentModel();
                    newComment.ProductId = selectedProduct.Id;
                    newComment.CommentHtml = textComments;
                    newComment.DisLikes = newComment.Likes = newComment.ExamId = newComment.HomeworkId = newComment.NewsId = newComment.BlogId = newComment.EventId = 0;
                    newComment.UserId = user.Id;
                    newComment.Username = user.UserName;
                    newComment.DisplayOrder = comments.Count + 1;
                    var commentEntity = newComment.ToEntity();
                    commentEntity.CreatedOn = commentEntity.ModifiedOn = DateTime.Now;
                    _commentService.Insert(commentEntity);

                    if (commentEntity.Id > 0)
                        selectedProduct.Comments.Add(commentEntity);

                    var searchReaction = _smsService.SearchReactions(productid: selectedProduct.Id, userid: user.Id).FirstOrDefault();
                    if ((frm[reactionSubmitted] != null && !string.IsNullOrEmpty(frm[reactionSubmitted].ToString())) || (frm[ratingSubmitted] != null && !string.IsNullOrEmpty(frm[ratingSubmitted].ToString())))
                    {
                        if (searchReaction == null)
                        {
                            searchReaction = new Reaction();
                            searchReaction.CreatedOn = searchReaction.ModifiedOn = DateTime.Now;
                        }

                        if (frm[reactionSubmitted] != null && !string.IsNullOrEmpty(frm[reactionSubmitted].ToString()))
                        {
                            switch (frm[reactionSubmitted].ToString().Trim().ToLower())
                            {
                                case "like":
                                    {
                                        searchReaction.IsLike = true;
                                        searchReaction.IsDislike = false;
                                        searchReaction.IsAngry = false;
                                        searchReaction.IsHappy = false;
                                        searchReaction.IsLOL = false;
                                        searchReaction.IsSad = false;
                                        break;
                                    }
                                case "dislike":
                                    {
                                        searchReaction.IsLike = false;
                                        searchReaction.IsDislike = true;
                                        searchReaction.IsAngry = false;
                                        searchReaction.IsHappy = false;
                                        searchReaction.IsLOL = false;
                                        searchReaction.IsSad = false;
                                        break;
                                    }
                                case "happy":
                                    {
                                        searchReaction.IsLike = false;
                                        searchReaction.IsDislike = false;
                                        searchReaction.IsAngry = false;
                                        searchReaction.IsHappy = true;
                                        searchReaction.IsLOL = false;
                                        searchReaction.IsSad = false;
                                        break;
                                    }
                                case "sad":
                                    {
                                        searchReaction.IsLike = false;
                                        searchReaction.IsDislike = false;
                                        searchReaction.IsAngry = false;
                                        searchReaction.IsHappy = false;
                                        searchReaction.IsLOL = false;
                                        searchReaction.IsSad = true;
                                        break;
                                    }
                                case "angry":
                                    {
                                        searchReaction.IsLike = false;
                                        searchReaction.IsDislike = false;
                                        searchReaction.IsAngry = true;
                                        searchReaction.IsHappy = false;
                                        searchReaction.IsLOL = false;
                                        searchReaction.IsSad = false;
                                        break;
                                    }
                                case "grin":
                                    {
                                        searchReaction.IsLike = false;
                                        searchReaction.IsDislike = false;
                                        searchReaction.IsAngry = false;
                                        searchReaction.IsHappy = false;
                                        searchReaction.IsLOL = true;
                                        searchReaction.IsSad = false;
                                        break;
                                    }
                                default:
                                    {
                                        searchReaction.IsLike = false;
                                        searchReaction.IsDislike = false;
                                        searchReaction.IsAngry = false;
                                        searchReaction.IsHappy = false;
                                        searchReaction.IsLOL = false;
                                        searchReaction.IsSad = false;
                                        break;
                                    }
                            }
                        }

                        if (frm[ratingSubmitted] != null && !string.IsNullOrEmpty(frm[ratingSubmitted].ToString()))
                        {
                            searchReaction.Rating = Convert.ToInt32(frm[ratingSubmitted].ToString());
                        }

                        searchReaction.PictureId = searchReaction.NewsId = searchReaction.ReplyId = searchReaction.VideoId = searchReaction.BlogId = searchReaction.CommentId = searchReaction.EventId = 0;
                        searchReaction.ProductId = selectedProduct.Id;
                        searchReaction.UserId = user.Id;
                        searchReaction.Username = _userService.GetUsernameByUser(user.Id);

                        if (searchReaction.Id == 0)
                        {
                            _smsService.InsertReaction(searchReaction);
                            selectedProduct.Reactions.Add(searchReaction);
                        }
                        else
                        {
                            _smsService.UpdateReaction(searchReaction);
                        }
                    }

                    // Update Product
                    _productService.Update(selectedProduct);
                }
                else
                {
                    var newUser = new UserModel();
                    string textComments = "";
                    var keyUsername = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "username");
                    if (keyUsername != null && !string.IsNullOrEmpty(frm[keyUsername].ToString()))
                        newUser.Username = frm[keyUsername].ToString();

                    var keyEmail = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "email");
                    if (keyEmail != null && !string.IsNullOrEmpty(frm[keyEmail].ToString()))
                        newUser.Email = frm[keyEmail].ToString();

                    var keyPassword = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "password");
                    if (keyPassword != null && !string.IsNullOrEmpty(frm[keyPassword].ToString()))
                        newUser.Password = frm[keyPassword].ToString();

                    var keyComment = frm.AllKeys.FirstOrDefault(x => x.Trim().ToLower() == "txtcomment");
                    if (keyComment != null && !string.IsNullOrEmpty(frm[keyComment].ToString()))
                        textComments = frm[keyComment].ToString();

                    if (!string.IsNullOrEmpty(newUser.Username)
                        && !string.IsNullOrEmpty(newUser.Email)
                        && !string.IsNullOrEmpty(newUser.Password)
                        && !string.IsNullOrEmpty(textComments))
                    {
                        newUser.IsActive = true;
                        newUser.IsApproved = true;
                        newUser.IsBlocked = false;
                        newUser.UserGuid = Guid.NewGuid();
                        var userEntity = newUser.ToEntity();
                        userEntity.CreatedOn = userEntity.ModifiedOn = DateTime.Now;

                        var defaultRole = _roleService.GetRoleByName("General");
                        userEntity.Roles.Add(defaultRole);
                        _userService.Insert(userEntity);

                        // Impersonate Relation
                        var teacher = _smsService.GetTeacherByImpersonateId(userEntity.Id);
                        var student = _smsService.GetStudentByImpersonateId(userEntity.Id);

                        //sign in customer
                        _authenticationService.SignIn(userEntity, false);
                        _userContext.CurrentUser = userEntity;

                        var comments = _commentService.GetCommentsByProduct(selectedProduct.Id);
                        comments = comments.OrderBy(x => x.DisplayOrder).ToList();

                        var newComment = new CommentModel();
                        newComment.ProductId = selectedProduct.Id;
                        newComment.CommentHtml = textComments;
                        newComment.Likes = newComment.DisLikes = 0;
                        newComment.ExamId = newComment.HomeworkId = newComment.NewsId = newComment.EventId = newComment.BlogId = 0;
                        newComment.UserId = userEntity.Id;
                        newComment.Username = userEntity.UserName;
                        newComment.DisplayOrder = comments.Count + 1;
                        var commentEntity = newComment.ToEntity();
                        commentEntity.CreatedOn = commentEntity.ModifiedOn = DateTime.Now;
                        _commentService.Insert(commentEntity);

                        if (commentEntity.Id > 0)
                            selectedProduct.Comments.Add(commentEntity);

                        var searchReaction = new Reaction();
                        if ((frm[reactionSubmitted] != null && !string.IsNullOrEmpty(frm[reactionSubmitted].ToString())) || (frm[ratingSubmitted] != null && !string.IsNullOrEmpty(frm[ratingSubmitted].ToString())))
                        {
                            searchReaction.CreatedOn = searchReaction.ModifiedOn = DateTime.Now;
                            if (frm[reactionSubmitted] != null && !string.IsNullOrEmpty(frm[reactionSubmitted].ToString()))
                            {
                                switch (frm[reactionSubmitted].ToString().Trim().ToLower())
                                {
                                    case "like":
                                        {
                                            searchReaction.IsLike = true;
                                            searchReaction.IsDislike = false;
                                            searchReaction.IsAngry = false;
                                            searchReaction.IsHappy = false;
                                            searchReaction.IsLOL = false;
                                            searchReaction.IsSad = false;
                                            break;
                                        }
                                    case "dislike":
                                        {
                                            searchReaction.IsLike = false;
                                            searchReaction.IsDislike = true;
                                            searchReaction.IsAngry = false;
                                            searchReaction.IsHappy = false;
                                            searchReaction.IsLOL = false;
                                            searchReaction.IsSad = false;
                                            break;
                                        }
                                    case "happy":
                                        {
                                            searchReaction.IsLike = false;
                                            searchReaction.IsDislike = false;
                                            searchReaction.IsAngry = false;
                                            searchReaction.IsHappy = true;
                                            searchReaction.IsLOL = false;
                                            searchReaction.IsSad = false;
                                            break;
                                        }
                                    case "sad":
                                        {
                                            searchReaction.IsLike = false;
                                            searchReaction.IsDislike = false;
                                            searchReaction.IsAngry = false;
                                            searchReaction.IsHappy = false;
                                            searchReaction.IsLOL = false;
                                            searchReaction.IsSad = true;
                                            break;
                                        }
                                    case "angry":
                                        {
                                            searchReaction.IsLike = false;
                                            searchReaction.IsDislike = false;
                                            searchReaction.IsAngry = true;
                                            searchReaction.IsHappy = false;
                                            searchReaction.IsLOL = false;
                                            searchReaction.IsSad = false;
                                            break;
                                        }
                                    case "grin":
                                        {
                                            searchReaction.IsLike = false;
                                            searchReaction.IsDislike = false;
                                            searchReaction.IsAngry = false;
                                            searchReaction.IsHappy = false;
                                            searchReaction.IsLOL = true;
                                            searchReaction.IsSad = false;
                                            break;
                                        }
                                    default:
                                        {
                                            searchReaction.IsLike = false;
                                            searchReaction.IsDislike = false;
                                            searchReaction.IsAngry = false;
                                            searchReaction.IsHappy = false;
                                            searchReaction.IsLOL = false;
                                            searchReaction.IsSad = false;
                                            break;
                                        }
                                }
                            }

                            if (frm[ratingSubmitted] != null && !string.IsNullOrEmpty(frm[ratingSubmitted].ToString()))
                            {
                                searchReaction.Rating = Convert.ToInt32(frm[ratingSubmitted].ToString());
                            }

                            searchReaction.PictureId = searchReaction.NewsId = searchReaction.ReplyId = searchReaction.VideoId = searchReaction.BlogId = searchReaction.CommentId = searchReaction.EventId = null;
                            searchReaction.ProductId = selectedProduct.Id;
                            searchReaction.UserId = userEntity.Id;
                            searchReaction.Username = userEntity.UserName;

                            if (searchReaction.Id == 0)
                            {
                                _smsService.InsertReaction(searchReaction);
                                selectedProduct.Reactions.Add(searchReaction);
                            }
                            else
                            {
                                _smsService.UpdateReaction(searchReaction);
                            }
                        }

                        // Update Product
                        _productService.Update(selectedProduct);
                    }
                }

                SuccessNotification("Reactions Added Successfully!");
            }
            return RedirectToRoute("Root");
        }

    }
}
