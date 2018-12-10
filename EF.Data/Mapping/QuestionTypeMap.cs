using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class QuestionTypeMap : CMSEntityTypeConfiguration<QuestionType>
    {
        public QuestionTypeMap()
        {
            this.ToTable("QuestionType");
            this.HasKey(b => b.Id);
            this.Property(b => b.Description).IsOptional();
            this.Property(b => b.Name).IsRequired();

            EntityTracker.TrackAllProperties<QuestionType>().Except(x => x.CreatedOn).And(x => x.ModifiedOn);

        }
    }
}
