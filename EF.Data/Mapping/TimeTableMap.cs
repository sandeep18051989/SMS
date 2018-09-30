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
			this.HasRequired(all => all.DivisionSubject).WithMany().HasForeignKey(all => all.DivisionSubjectId);
			this.HasRequired(all => all.Teacher).WithMany().HasForeignKey(all => all.TeacherId);
			EntityTracker.TrackAllProperties<TimeTable>().Except(x => x.Teacher).And(x => x.DivisionSubject).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
