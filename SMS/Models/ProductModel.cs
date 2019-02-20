using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;
using SMS.Models.Widgets;

namespace SMS.Models
{
    [Validator(typeof(ProductValidator))]
    public partial class ProductModel : BaseEntityModel
    {
        public ProductModel()
        {
            Videos = new List<ProductVideoModel>();
            Pictures = new List<ProductPictureModel>();
            Comments = new List<CommentWidgetModel>();
            Files = new List<FilesModel>();
            Reactions = new List<ReactionModel>();
            AvailableVendors = new List<SelectListItem>();
            AvailableAcadmicYears = new List<SelectListItem>();
            FeaturedProducts = new List<ProductModel>();
            NewProducts = new List<ProductModel>();
            UpcomingProducts = new List<ProductModel>();
            RelatedProducts = new List<ProductModel>();
        }
        public string Name { get; set; }
        public string SystemName { get; set; }
        [AllowHtml]
        [UIHint("HtmlEditor")]
        public string Description { get; set; }
        public string Url { get; set; }
        public string SeoName { get; set; }
        [UIHint("File")]
        public int FileId { get; set; }
        public bool IsAuthenticated { get; set; }
        public int? VendorId { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        [UIHint("DateRange")]
        public DateTime? AvailableStartDate { get; set; }
        [UIHint("DateRange")]
        public DateTime? AvailableEndDate { get; set; }
        public bool IsUpcoming { get; set; }
        public bool MarkAsNew { get; set; }
        [UIHint("DateRange")]
        public DateTime? MarkAsNewStartDate { get; set; }
        [UIHint("DateRange")]
        public DateTime? MarkAsNewEndDate { get; set; }
        public double? OldPrice { get; set; }
        public double Price { get; set; }
        public double BasePrice { get; set; }
        public bool DisableBuyButton { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool Selected { get; set; }
        [UIHint("Picture")]
        public int PictureId { get; set; }
        [UIHint("Video")]
        public int VideoId { get; set; }
        public int? AcadmicYearId { get; set; }
        public string DefaultPictureSrc { get; set; }
        public string DefaultVideoSrc { get; set; }
        public bool HasDefaultPicture { get; set; }
        public bool HasDefaultVideo { get; set; }
        public string ProductCategory { get; set; }
        public IList<ProductVideoModel> Videos { get; set; }
        public IList<ProductPictureModel> Pictures { get; set; }
        public IList<FilesModel> Files { get; set; }
        public IList<CommentWidgetModel> Comments { get; set; }
        public IList<ReactionModel> Reactions { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }
        public IList<SelectListItem> AvailableAcadmicYears { get; set; }

        public IList<ProductModel> FeaturedProducts { get; set; }
        public IList<ProductModel> NewProducts { get; set; }
        public IList<ProductModel> UpcomingProducts { get; set; }

        public IList<ProductModel> RelatedProducts { get; set; }
    }

    public partial class ProductListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string AcadmicYear { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsApproved { get; set; }
        public bool IsClosed { get; set; }
        public string Url { get; set; }
        public int FilesCount { get; set; }
        public int VideosCount { get; set; }
        public int PicturesCount { get; set; }
        public int CommentsCount { get; set; }
        public int ReactionsCount { get; set; }
        public string Price { get; set; }
    }
}