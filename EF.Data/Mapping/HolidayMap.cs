using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class HolidayMap : CMSEntityTypeConfiguration<Holiday>
	{
		public HolidayMap()
		{
			this.ToTable("Holiday");
			this.HasKey(ho => ho.Id);
			EntityTracker.TrackAllProperties<File>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);
		}
	}
}
