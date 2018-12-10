using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ClassExamMap : CMSEntityTypeConfiguration<ClassExam>
	{

		public ClassExamMap()
		{
			this.ToTable("Class_Exam_Mapping");
			this.HasKey(b => b.Id);
            this.Property(b => b.BreakAllowed).IsOptional();
            this.Property(b => b.BreakTime).IsOptional();
            this.Property(b => b.ClassId).IsRequired();
            this.Property(b => b.ClassRoomId).IsRequired();
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.EndTime).IsOptional();
            this.Property(b => b.ExamId).IsRequired();
            this.Property(b => b.GradeSystemId).IsRequired();
            this.Property(b => b.MarksObtained).IsOptional();
            this.Property(b => b.ResultStatusId).IsOptional();
            this.Property(b => b.StartDate).IsOptional();
            this.Property(b => b.StartTime).IsOptional();

            // Relationships
            this.HasRequired(ca => ca.Class).WithMany().HasForeignKey(ca => ca.ClassId);
			this.HasRequired(ca => ca.Exam).WithMany().HasForeignKey(ca => ca.ExamId);
			this.HasRequired(ca => ca.ClassRoom).WithMany().HasForeignKey(ca => ca.ClassRoomId);

			EntityTracker.TrackAllProperties<ClassExam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.ClassRoom).And(x => x.Class).And(x => x.Exam);

		}
	}
}
