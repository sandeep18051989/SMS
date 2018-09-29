using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class SubjectExamMap : CMSEntityTypeConfiguration<SubjectExam>
	{

		public SubjectExamMap()
		{
			this.ToTable("Subject_Exam_Mapping");
			this.HasKey(b => b.Id);

			// Relationships
			this.HasRequired(ca => ca.Subject).WithMany().HasForeignKey(ca => ca.SubjectId);
			this.HasRequired(ca => ca.Exam).WithMany().HasForeignKey(ca => ca.ExamId);
			EntityTracker.TrackAllProperties<SubjectExam>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.Subject).And(x => x.Exam);

		}
	}
}
