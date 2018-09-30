using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class BlogPictureMap : CMSEntityTypeConfiguration<BlogPicture>
	{
		public BlogPictureMap()
		{
			this.ToTable("BlogPicture");
			this.HasKey(e => e.Id);
			this.Property(e => e.StartDate).IsOptional();
			this.Property(e => e.EndDate).IsOptional();

			// Relationships
			this.HasRequired(e => e.Blog).WithMany(p => p.Pictures).HasForeignKey(e => e.BlogId);
			this.HasRequired(e => e.Picture).WithMany(p => p.Blogs).HasForeignKey(e => e.PictureId);

			EntityTracker.TrackAllProperties<BlogPicture>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.Blog).And(x => x.Picture);
        }
	}
}
