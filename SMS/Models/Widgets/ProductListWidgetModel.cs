using System.Collections.Generic;

namespace SMS.Models.Widgets
{
    public partial class ProductListWidgetModel
    {
		public ProductListWidgetModel()
		{
            Products = new List<ProductModel>();
        }
        public string KeyWord { get; set; }
        public IList<ProductModel> Products { get; set; }
    }
}