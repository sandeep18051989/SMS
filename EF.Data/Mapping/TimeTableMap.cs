using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class TimeTableMap : CMSEntityTypeConfiguration<TimeTable>
	{

		public TimeTableMap()
		{
			this.ToTable("Time_Table");
			this.HasKey(b => b.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.DivisionSubjectId).IsRequired();
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.EndTime).IsOptional();
            this.Property(b => b.LectureNumber).IsOptional();
            this.Property(b => b.RecessAllowed).IsOptional();
            this.Property(b => b.StartDate).IsOptional();
            this.Property(b => b.StartTime).IsOptional();
            this.Property(b => b.TeacherId).IsRequired();
            this.Property(b => b.WeekDayId).IsOptional();

            this.HasRequired(all => all.DivisionSubject).WithMany().HasForeignKey(all => all.DivisionSubjectId);
			this.HasRequired(all => all.Teacher).WithMany().HasForeignKey(all => all.TeacherId);
			EntityTracker.TrackAllProperties<TimeTable>().Except(x => x.Teacher).And(x => x.DivisionSubject).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
