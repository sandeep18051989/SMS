using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class StudentExamMap : CMSEntityTypeConfiguration<StudentExam>
	{
		public StudentExamMap()
		{
			this.ToTable("Student_Exam_Mapping");
			this.HasKey(b => b.Id);
            this.Property(b => b.StudentId).IsRequired();
            this.Property(b => b.ExamId).IsRequired();
            this.Property(b => b.BreakAllowed).IsOptional();
            this.Property(b => b.BreakTime).IsOptional();
            this.Property(b => b.ClassRoomId).IsRequired();
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.EndTime).IsOptional();
            this.Property(b => b.GradeSystemId).IsOptional();
            this.Property(b => b.MarksObtained).IsOptional();
            this.Property(b => b.ResultStatusId).IsOptional();
            this.Property(b => b.StartDate).IsOptional();
            this.Property(b => b.StartTime).IsOptional();

            this.HasRequired(all => all.Student).WithMany().HasForeignKey(all => all.StudentId);
			this.HasRequired(all => all.Exam).WithMany().HasForeignKey(all => all.ExamId);
			this.HasRequired(all => all.ClassRoom).WithMany().HasForeignKey(all => all.ClassRoomId);

			EntityTracker.TrackAllProperties<StudentExam>().Except(x => x.Student).And(x => x.Exam).And(x => x.ClassRoom).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
