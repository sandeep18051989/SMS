using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ProductMap : CMSEntityTypeConfiguration<Product>
	{
		public ProductMap()
		{
			this.ToTable("Product");
			this.HasKey(pro => pro.Id);
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.VendorId).IsOptional();
            this.Property(b => b.MetaDescription).IsOptional();
            this.Property(b => b.MetaKeywords).IsOptional();
            this.Property(b => b.MetaTitle).IsOptional();
            this.Property(b => b.MarkAsNewStartDate).IsOptional();
            this.Property(b => b.MarkAsNewEndDate).IsOptional();
            this.Property(b => b.MarkAsNew).IsOptional();
            this.Property(b => b.IsUpcoming).IsOptional();
            this.Property(b => b.DisableBuyButton).IsOptional();
            this.Property(b => b.AvailableEndDate).IsOptional();
            this.Property(b => b.AvailableStartDate).IsOptional();
            this.Property(b => b.AcadmicYearId).IsOptional();
            this.Property(b => b.OldPrice).IsOptional();
            this.Property(b => b.BasePrice).IsRequired();
            this.Property(b => b.Price).IsRequired();
            this.Property(b => b.StockQuantity).IsRequired();

            // Relationships
            this.HasMany(pro => pro.Files).WithMany(p => p.Products).Map(m => m.ToTable("Product_File_Map").MapLeftKey("ProductId").MapRightKey("FileId"));
			this.HasMany(pro => pro.Comments).WithMany(p => p.Products).Map(m => m.ToTable("Product_Comment_Map").MapLeftKey("ProductId").MapRightKey("CommentId"));

			EntityTracker.TrackAllProperties<Product>().Except(x => x.Pictures).And(x => x.AcadmicYearId).And(x => x.ModifiedOn).And(x => x.CreatedOn);

		}

	}
}
