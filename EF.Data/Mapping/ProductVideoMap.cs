using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ProductVideoMap : CMSEntityTypeConfiguration<ProductVideo>
	{
		public ProductVideoMap()
		{
			this.ToTable("ProductVideo");
			this.HasKey(e => e.Id);
			this.Property(e => e.StartDate).IsOptional();
			this.Property(e => e.EndDate).IsOptional();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.ProductId).IsRequired();
            this.Property(b => b.VideoId).IsRequired();

            // Relationships
            this.HasRequired(e => e.Product).WithMany(p => p.Videos).HasForeignKey(e => e.ProductId);
			this.HasRequired(e => e.Video).WithMany(p => p.Products).HasForeignKey(e => e.VideoId);

			EntityTracker.TrackAllProperties<ProductVideo>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.Product).And(x => x.Video);
		}
	}
}
