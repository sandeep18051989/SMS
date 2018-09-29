using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class AssessmentQuestionMap : CMSEntityTypeConfiguration<AssessmentQuestion>
	{
		public AssessmentQuestionMap()
		{
			this.ToTable("Assessment_Question_Mapping");
			this.HasKey(b => b.Id);

			// Relationship
			this.HasRequired(qu => qu.Assessment)
				.WithMany()
				.HasForeignKey(qu => qu.AssessmentId);
			this.HasRequired(qu => qu.Question)
				.WithMany()
				.HasForeignKey(qu => qu.QuestionId);


			EntityTracker.TrackAllProperties<AssessmentQuestion>().Except(x => x.Assessment).And(x => x.Question).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
