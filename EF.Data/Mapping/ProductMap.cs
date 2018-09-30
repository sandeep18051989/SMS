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

			// Relationships
			this.HasMany(pro => pro.Videos).WithMany(p => p.Products).Map(m => m.ToTable("Product_Video_Map").MapLeftKey("ProductId").MapRightKey("VideoId"));
			this.HasMany(pro => pro.Files).WithMany(p => p.Products).Map(m => m.ToTable("Product_File_Map").MapLeftKey("ProductId").MapRightKey("FileId"));
			this.HasMany(pro => pro.Comments).WithMany(p => p.Products).Map(m => m.ToTable("Product_Comment_Map").MapLeftKey("ProductId").MapRightKey("CommentId"));

			this.HasRequired(cust => cust.Vendor).WithMany().HasForeignKey(cust => cust.VendorId);
			EntityTracker.TrackAllProperties<Product>().Except(x => x.Pictures).And(x => x.AcadmicYearId).And(x => x.Vendor).And(x => x.ModifiedOn).And(x => x.CreatedOn);

		}

	}
}
