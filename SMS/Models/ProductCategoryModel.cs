using System.Collections.Generic;
using System.Web.Mvc;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
    [Validator(typeof(ProductCategoryModelValidator))]
    public partial class ProductCategoryModel : BaseEntityModel
    {
        public ProductCategoryModel()
        {
            ProductCategory = new ProductSubCategoryModel();
            AvailableProductCategories = new List<SelectListItem>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentCategoryId { get; set; }
        public int PictureId { get; set; }
        public bool IncludeInTopMenu { get; set; }
        public bool IsActive { get; set; }
        public bool Selected { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
        public string SystemName { get; set; }
        public string PictureSrc { get; set; }
        public string Url { get; set; }

        public ProductSubCategoryModel ProductCategory { get; set; }
        public IList<SelectListItem> AvailableProductCategories { get; set; }

    }

    public partial class ProductSubCategoryModel : BaseEntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentCategoryId { get; set; }
        public int PictureId { get; set; }
        public bool IncludeInTopMenu { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
        public string SystemName { get; set; }
        public string PictureSrc { get; set; }
        public string Url { get; set; }

    }
}
