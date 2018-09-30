using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class TimeTableSettingMap : CMSEntityTypeConfiguration<TimeTableSetting>
	{

		public TimeTableSettingMap()
		{
			this.ToTable("Time_Table_Setting");
			this.HasKey(b => b.Id);
			EntityTracker.TrackAllProperties<TimeTableSetting>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
