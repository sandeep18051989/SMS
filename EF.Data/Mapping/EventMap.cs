using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class EventMap : CMSEntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            this.ToTable("Event");
            this.HasKey(e => e.Id);
            this.Property(e => e.StartDate).IsOptional();
            this.Property(e => e.EndDate).IsOptional();
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Headline).IsRequired();
            this.Property(b => b.Latitude).IsOptional();
            this.Property(b => b.Longitude).IsOptional();
            this.Property(b => b.SystemName).IsOptional();
            this.Property(b => b.Title).IsRequired();
            this.Property(b => b.Venue).IsOptional();

            // Relationships
            this.HasMany(e => e.Comments).WithMany(c=>c.Events).Map(m => m.ToTable("Event_Comment_Map").MapLeftKey("EventId").MapRightKey("CommentId"));
            EntityTracker.TrackAllProperties<Event>().Except(x => x.Videos).And(x => x.Comments).And(x => x.Pictures).And(x => x.ModifiedOn).And(x => x.CreatedOn);
        }
    }
}
