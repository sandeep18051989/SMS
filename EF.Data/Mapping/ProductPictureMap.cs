using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ProductPictureMap : CMSEntityTypeConfiguration<ProductPicture>
	{
		public ProductPictureMap()
		{
			this.ToTable("ProductPicture");
			this.HasKey(e => e.Id);
			this.Property(e => e.StartDate).IsOptional();
			this.Property(e => e.EndDate).IsOptional();

			// Relationships
			this.HasRequired(e => e.Product).WithMany(p => p.Pictures).HasForeignKey(e => e.ProductId);
			this.HasRequired(e => e.Picture).WithMany(p => p.Products).HasForeignKey(e => e.PictureId);

			EntityTracker.TrackAllProperties<ProductPicture>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.Picture).And(x => x.Product);
		}
	}
}
