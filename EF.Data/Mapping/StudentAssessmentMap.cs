using System;
using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class StudentAssessmentMap : CMSEntityTypeConfiguration<StudentAssessment>
	{

		public StudentAssessmentMap()
		{
			this.ToTable("Student_Assessment_Mapping");
			this.HasKey(b => b.Id);

			// Relationships
			this.HasRequired(ca => ca.Student).WithMany().HasForeignKey(ca => ca.StudentId);
			this.HasRequired(ca => ca.Assessment).WithMany().HasForeignKey(ca => ca.AssessmentId);

			EntityTracker.TrackAllProperties<StudentAssessment>().Except(x => x.CreatedOn).And(x => x.ModifiedOn).And(x => x.Student).And(x => x.Assessment);

		}
	}
}
