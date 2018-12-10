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
            this.Property(b => b.AssessmentId).IsRequired();
            this.Property(b => b.QuestionId).IsRequired();
            this.Property(b => b.NegativeMarks).IsOptional();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.SolveTime).IsOptional();
            this.Property(b => b.RightMarks).IsOptional();

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
