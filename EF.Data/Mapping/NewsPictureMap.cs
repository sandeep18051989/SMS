using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class NewsPictureMap : CMSEntityTypeConfiguration<NewsPicture>
	{
		public NewsPictureMap()
		{
			this.ToTable("NewsPicture");
			this.HasKey(e => e.Id);
			this.Property(e => e.StartDate).IsOptional();
			this.Property(e => e.EndDate).IsOptional();


			// Relationships
			this.HasRequired(e => e.News).WithMany(p => p.Pictures).HasForeignKey(e => e.NewsId);
			this.HasRequired(e => e.Picture).WithMany(p => p.News).HasForeignKey(e => e.PictureId);

			EntityTracker.TrackAllProperties<NewsPicture>().Except(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.Picture).And(x => x.News);
        }
	}
}
