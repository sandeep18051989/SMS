using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class QuestionMap : CMSEntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            this.ToTable("Question");
            this.HasKey(b => b.Id);
            this.Property(b => b.SubjectId).IsOptional();
            this.Property(b => b.SolveTime).IsOptional();

            // Relationship
            this.HasRequired(qu => qu.QuestionType)
		        .WithMany()
		        .HasForeignKey(qu => qu.QuestionTypeId);
	        this.HasOptional(qu => qu.Subject)
		        .WithMany()
		        .HasForeignKey(qu => qu.SubjectId);

			EntityTracker.TrackAllProperties<Question>().Except(x => x.CreatedOn).And(x => x.Subject).And(x => x.QuestionType).And(x => x.ModifiedOn);

        }
    }
}
