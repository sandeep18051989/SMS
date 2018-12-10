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
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.LectureTime).IsOptional();
            this.Property(b => b.NoBreak).IsOptional();
            this.Property(b => b.RecessTimeMin).IsOptional();
            this.Property(b => b.SchoolStartTime).IsOptional();
            this.Property(b => b.SchoolEndTime).IsOptional();

            EntityTracker.TrackAllProperties<TimeTableSetting>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
