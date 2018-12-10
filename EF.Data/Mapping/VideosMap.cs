using System.Data.Entity.ModelConfiguration;
using EF.Core.Data;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class VideosMap : EntityTypeConfiguration<Video>
    {
        public VideosMap()
        {
            this.ToTable("Video");
            this.HasKey(v => v.Id);
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.Size).IsOptional();
            this.Property(b => b.Url).IsOptional();
            this.Property(b => b.VideoSrc).IsRequired();

            EntityTracker.TrackAllProperties<Video>().Except(x => x.Blogs).And(x => x.Events).And(x => x.Products).And(x => x.ModifiedOn).And(x => x.CreatedOn);
        }
    }
}
