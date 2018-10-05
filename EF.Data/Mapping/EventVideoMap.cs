using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class EventVideoMap : CMSEntityTypeConfiguration<EventVideo>
	{
		public EventVideoMap()
		{
			this.ToTable("EventVideo");
			this.HasKey(e => e.Id);
			this.Property(e => e.StartDate).IsOptional();
			this.Property(e => e.EndDate).IsOptional();

			// Relationships
			this.HasRequired(e => e.Event).WithMany(p => p.Videos).HasForeignKey(e => e.EventId);
			this.HasRequired(e => e.Video).WithMany(p => p.Events).HasForeignKey(e => e.VideoId);

			EntityTracker.TrackAllProperties<EventVideo>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.Event).And(x => x.Video);
		}
	}
}
