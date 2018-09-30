using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class ProductCategoryMappingMap : CMSEntityTypeConfiguration<ProductCategoryMapping>
	{
		public ProductCategoryMappingMap()
		{
			this.ToTable("Product_Category_Mapping");
			this.HasKey(pro => pro.Id);

            this.HasRequired(pc => pc.ProductCategory)
                .WithMany()
                .HasForeignKey(pc => pc.ProductCategoryId);


            this.HasRequired(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            EntityTracker.TrackAllProperties<ProductCategoryMapping>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.Product).And(x => x.ProductCategory);

		}

	}
}
