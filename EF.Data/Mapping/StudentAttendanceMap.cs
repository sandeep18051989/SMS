using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class StudentAttendanceMap : CMSEntityTypeConfiguration<StudentAttendance>
	{
		public StudentAttendanceMap()
		{
			this.ToTable("Student_Attendance");
			this.HasKey(b => b.Id);
            this.Property(b => b.StudentId).IsRequired();
            this.Property(b => b.AttendanceStatusId).IsOptional();
            this.Property(b => b.DD).IsOptional();
            this.Property(b => b.Date).IsOptional();
            this.Property(b => b.MM).IsOptional();
            this.Property(b => b.YYYY).IsOptional();
            this.Property(b => b.IsHoliday).IsOptional();

            this.HasRequired(all => all.Student).WithMany().HasForeignKey(all => all.StudentId);

			EntityTracker.TrackAllProperties<StudentAttendance>().Except(x => x.Student).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
