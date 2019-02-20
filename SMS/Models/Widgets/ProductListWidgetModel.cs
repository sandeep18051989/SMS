using EF.Services;
using System.Collections.Generic;

namespace SMS.Models.Widgets
{
    public partial class ProductListWidgetModel
    {
		public ProductListWidgetModel()
		{
            Products = new List<ProductModel>();
            ProductCategories = new List<ProductCategoryModel>();
            PagingFilteringContext = new PagingFilteringModel();
        }
        public IList<ProductModel> Products { get; set; }
        public PagingFilteringModel PagingFilteringContext { get; set; }
        public IList<ProductCategoryModel> ProductCategories { get; set; }
    }

    public partial class PagingFilteringModel : BasePageModel
    {
    }
}