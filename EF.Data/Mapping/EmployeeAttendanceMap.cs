using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class EmployeeAttendanceMap : CMSEntityTypeConfiguration<EmployeeAttendance>
	{
		public EmployeeAttendanceMap()
		{
			this.ToTable("Employee_Attendance");
			this.HasKey(b => b.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.AttendanceStatusId).IsOptional();
            this.Property(b => b.Date).IsRequired();
            this.Property(b => b.DD).IsRequired();
            this.Property(b => b.EmployeeId).IsOptional();
            this.Property(b => b.IsHoliday).IsOptional();
            this.Property(b => b.MM).IsRequired();
            this.Property(b => b.YYYY).IsRequired();

            this.HasRequired(all => all.Employee).WithMany().HasForeignKey(all => all.EmployeeId);
			EntityTracker.TrackAllProperties<EmployeeAttendance>().Except(x => x.Employee).And(x => x.CreatedOn).And(x => x.ModifiedOn);
		}
	}
}
