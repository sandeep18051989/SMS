using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ProductCategoryMap : CMSEntityTypeConfiguration<ProductCategory>
	{
		public ProductCategoryMap()
		{
			this.ToTable("ProductCategory");
			this.HasKey(pro => pro.Id);
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.IncludeInTopMenu).IsOptional();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.ParentCategoryId).IsOptional();
            this.Property(b => b.PictureId).IsOptional();

            EntityTracker.TrackAllProperties<ProductCategory>().Except(x => x.ModifiedOn).And(x => x.CreatedOn);

		}

	}
}
