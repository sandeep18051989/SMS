using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class ProductCategory : BaseEntity , ISlugSupported
    {
		public string Name { get; set; }
        public string Description { get; set; }
        public int ParentCategoryId { get; set; }
        public int PictureId { get; set; }
        public bool IncludeInTopMenu { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }

    }

    public partial class ProductCategoryMapping : BaseEntity
    {
        public int ProductCategoryId { get; set; }
        public int ProductId { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ProductCategory ProductCategory { get; set;}
        public virtual Product Product { get; set; }
    }
}
