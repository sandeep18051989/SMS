using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class NewsVideoMap : CMSEntityTypeConfiguration<NewsVideo>
	{
		public NewsVideoMap()
		{
			this.ToTable("NewsVideo");
			this.HasKey(e => e.Id);
            this.Property(e => e.StartDate).IsOptional();
            this.Property(e => e.EndDate).IsOptional();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.NewsId).IsRequired();
            this.Property(b => b.VideoId).IsRequired();


            // Relationships
            this.HasRequired(e => e.News).WithMany(p => p.Videos).HasForeignKey(e => e.NewsId);
			this.HasRequired(e => e.Video).WithMany(p => p.News).HasForeignKey(e => e.VideoId);

			EntityTracker.TrackAllProperties<NewsVideo>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.News).And(x => x.Video);
		}
	}
}
