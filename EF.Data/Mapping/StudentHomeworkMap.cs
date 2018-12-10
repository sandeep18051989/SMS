using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class StudentHomeworkMap : CMSEntityTypeConfiguration<StudentHomework>
	{

		public StudentHomeworkMap()
		{
			this.ToTable("Student_Homework_Mapping");
			this.HasKey(b => b.Id);
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.HomeworkId).IsRequired();
            this.Property(b => b.StartDate).IsOptional();
            this.Property(b => b.StudentId).IsRequired();

            // Relationships
            this.HasRequired(ca => ca.Student).WithMany().HasForeignKey(ca => ca.StudentId);
			this.HasRequired(ca => ca.Homework).WithMany().HasForeignKey(ca => ca.HomeworkId);

			EntityTracker.TrackAllProperties<StudentHomework>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.Student).And(x => x.Homework);

		}
	}
}
