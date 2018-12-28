using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class TeacherExamMap : CMSEntityTypeConfiguration<TeacherExam>
	{

		public TeacherExamMap()
		{
			this.ToTable("Teacher_Exam");
			this.HasKey(b => b.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.BreakAllowed).IsOptional();
            this.Property(b => b.BreakTime).IsOptional();
            this.Property(b => b.ClassRoomId).IsRequired();
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.EndTime).IsOptional();
            this.Property(b => b.ExamId).IsRequired();
            this.Property(b => b.GradeSystemId).IsOptional();
            this.Property(b => b.MarksObtained).IsOptional();
            this.Property(b => b.ResultStatusId).IsOptional();
            this.Property(b => b.StartDate).IsOptional();
            this.Property(b => b.StartTime).IsOptional();
            this.Property(b => b.TeacherId).IsRequired();

            // Relationships
            this.HasMany(e => e.Comments).WithMany(c => c.TeacherExams).Map(m => m.ToTable("Teacher_Exam_Comment_Map").MapLeftKey("TeacherExamId").MapRightKey("CommentId"));
            this.HasRequired(ca => ca.Teacher).WithMany(e => e.TeacherExams).HasForeignKey(ca => ca.TeacherId);
			this.HasRequired(ca => ca.Exam).WithMany(e => e.TeacherExams).HasForeignKey(ca => ca.ExamId);
			this.HasRequired(ca => ca.ClassRoom).WithMany().HasForeignKey(ca => ca.ClassRoomId);
			EntityTracker.TrackAllProperties<TeacherExam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.ClassRoom).And(x => x.Teacher).And(x => x.Exam);

		}
	}
}
