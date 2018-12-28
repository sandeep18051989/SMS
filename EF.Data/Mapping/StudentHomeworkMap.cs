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
            this.HasMany(e => e.Comments).WithMany(c => c.StudentHomeworks).Map(m => m.ToTable("Student_Homework_Comment_Map").MapLeftKey("StudentHomeworkId").MapRightKey("CommentId"));

            this.HasRequired(ca => ca.Student).WithMany(e => e.StudentHomeworks).HasForeignKey(ca => ca.StudentId);
			this.HasRequired(ca => ca.Homework).WithMany(e => e.StudentHomeworks).HasForeignKey(ca => ca.HomeworkId);

			EntityTracker.TrackAllProperties<StudentHomework>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.Student).And(x => x.Homework);

		}
	}
}
