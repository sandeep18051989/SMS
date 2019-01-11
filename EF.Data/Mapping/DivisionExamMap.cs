using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class DivisionExamMap : CMSEntityTypeConfiguration<DivisionExam>
	{

		public DivisionExamMap()
		{
			this.ToTable("Division_Exam_Mapping");
			this.HasKey(b => b.Id);
            this.Property(b => b.BreakAllowed).IsOptional();
            this.Property(b => b.BreakTime).HasMaxLength(20).IsOptional();
            this.Property(b => b.ClassRoomId).IsRequired();
            this.Property(b => b.DivisionId).IsRequired();
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.EndTime).HasMaxLength(20).IsOptional();
            this.Property(b => b.ExamId).IsRequired();
            this.Property(b => b.GradeSystemId).IsOptional();
            this.Property(b => b.MarksObtained).IsOptional();
            this.Property(b => b.ResultStatusId).IsOptional();
            this.Property(b => b.StartDate).IsOptional();
            this.Property(b => b.PassingMarks).IsOptional();
            this.Property(b => b.MaxMarks).IsOptional();
            this.Property(b => b.StartTime).HasMaxLength(20).IsOptional();

            // Relationships
            this.HasMany(e => e.Comments).WithMany(c => c.DivisionExams).Map(m => m.ToTable("Division_Exam_Comment_Map").MapLeftKey("DivisionExamId").MapRightKey("CommentId"));
            this.HasRequired(ca => ca.Division).WithMany(e => e.DivisionExams).HasForeignKey(ca => ca.DivisionId);
            this.HasRequired(ca => ca.Exam).WithMany(e => e.DivisionExams).HasForeignKey(ca => ca.ExamId);
            this.HasRequired(ca => ca.ClassRoom).WithMany(e => e.DivisionExams).HasForeignKey(ca => ca.ClassRoomId);

            EntityTracker.TrackAllProperties<DivisionExam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.ClassRoom);

		}
	}
}
