using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
	public partial class OptionMap : CMSEntityTypeConfiguration<Option>
	{
		public OptionMap()
		{
			this.ToTable("Option");
			this.HasKey(b => b.Id);
            this.Property(b => b.CorrectAnswer).IsRequired();
            this.Property(b => b.DisplayOrder).IsOptional();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.QuestionId).IsRequired();

            // Relationship
            this.HasRequired(qu => qu.Question)
				.WithMany()
				.HasForeignKey(qu => qu.QuestionId);

			EntityTracker.TrackAllProperties<Option>().Except(x => x.Question).And(x => x.CreatedOn).And(x => x.ModifiedOn);

		}
	}
}
