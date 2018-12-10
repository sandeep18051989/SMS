using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ClassHomeworkMap : CMSEntityTypeConfiguration<ClassHomework>
	{

		public ClassHomeworkMap()
		{
			this.ToTable("Class_Homework_Mapping");
			this.HasKey(b => b.Id);
            this.Property(b => b.ClassId).IsRequired();
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.HomeworkId).IsRequired();
            this.Property(b => b.StartDate).IsOptional();

            // Relationships
            this.HasRequired(ca => ca.Class).WithMany().HasForeignKey(ca => ca.ClassId);
			this.HasRequired(ca => ca.Homework).WithMany().HasForeignKey(ca => ca.HomeworkId);

			EntityTracker.TrackAllProperties<ClassHomework>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.Class).And(x => x.Homework);

		}
	}
}
