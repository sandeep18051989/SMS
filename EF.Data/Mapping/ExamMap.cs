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

			this.HasMany(e => e.Comments).WithMany(c => c.Exams).Map(m => m.ToTable("Exam_Comment_Map").MapLeftKey("ExamId").MapRightKey("CommentId"));
			EntityTracker.TrackAllProperties<Exam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
