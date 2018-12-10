using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class ExamMap : CMSEntityTypeConfiguration<Exam>
	{
		public ExamMap()
		{
			this.ToTable("Exam");
			this.HasKey(b => b.Id);
            this.Property(b => b.AcadmicYearId).IsRequired();
            this.Property(b => b.EndDate).IsOptional();
            this.Property(b => b.ExamName).IsRequired();
            this.Property(b => b.MaxMarks).IsOptional();
            this.Property(b => b.PassingMarks).IsOptional();
            this.Property(b => b.StartDate).IsOptional();

            this.HasMany(e => e.Comments).WithMany(c => c.Exams).Map(m => m.ToTable("Exam_Comment_Map").MapLeftKey("ExamId").MapRightKey("CommentId"));
			EntityTracker.TrackAllProperties<Exam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
