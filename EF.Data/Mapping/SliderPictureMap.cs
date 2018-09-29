using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class SliderPictureMap : CMSEntityTypeConfiguration<Slider>
    {
        public SliderPictureMap()
        {
            this.ToTable("Slider");
            this.HasKey(sl => sl.Id);
            this.Property(sl => sl.IsActive);
            this.Property(sl => sl.UserId);

            this.HasMany(sl => sl.Pictures).
                WithMany()
                .Map(m => m.ToTable("Slider_Picture_Map").MapLeftKey("SliderId").MapRightKey("PictureId"));

            EntityTracker.TrackAllProperties<Slider>().Except(x => x.Pictures).And(x => x.ModifiedOn).And(x => x.CreatedOn);

        }
    }
}
