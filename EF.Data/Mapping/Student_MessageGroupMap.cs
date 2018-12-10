using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class Student_MessageGroupMap : CMSEntityTypeConfiguration<Student_MessageGroup>
	{
		public Student_MessageGroupMap()
		{
			this.ToTable("Student_MessageGroup");
			this.HasKey(b => b.Id);
            this.Property(b => b.StudentId).IsRequired();
            this.Property(b => b.MessageGroupId).IsRequired();
            this.Property(b => b.DDate).IsOptional();

            this.HasRequired(all => all.Student).WithMany().HasForeignKey(all => all.StudentId);
			this.HasRequired(all => all.MessageGroup).WithMany().HasForeignKey(all => all.MessageGroupId);

			EntityTracker.TrackAllProperties<Student_MessageGroup>().Except(x => x.Student).And(x => x.MessageGroup).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
