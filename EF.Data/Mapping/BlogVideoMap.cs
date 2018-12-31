using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class BlogVideoMap : CMSEntityTypeConfiguration<BlogVideo>
	{
		public BlogVideoMap()
		{
			this.ToTable("BlogVideo");
			this.HasKey(e => e.Id);
			this.Property(e => e.StartDate).IsOptional();
			this.Property(e => e.EndDate).IsOptional();

			// Relationships
			this.HasRequired(e => e.Blog).WithMany(p => p.Videos).HasForeignKey(e => e.BlogId);
			this.HasRequired(e => e.Video).WithMany(p => p.Blogs).HasForeignKey(e => e.VideoId);

			EntityTracker.TrackAllProperties<BlogVideo>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.Blog).And(x => x.Video);
		}
	}
}
