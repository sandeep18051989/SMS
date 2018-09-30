using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class EventPictureMap : CMSEntityTypeConfiguration<EventPicture>
	{
		public EventPictureMap()
		{
			this.ToTable("EventPicture");
			this.HasKey(e => e.Id);
			this.Property(e => e.StartDate).IsOptional();
			this.Property(e => e.EndDate).IsOptional();

			// Relationships
			this.HasRequired(e => e.Event).WithMany(p => p.Pictures).HasForeignKey(e => e.EventId);
			this.HasRequired(e => e.Picture).WithMany(p => p.Events).HasForeignKey(e => e.PictureId);

			EntityTracker.TrackAllProperties<EventPicture>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.Event).And(x => x.Picture);
		}
	}
}
