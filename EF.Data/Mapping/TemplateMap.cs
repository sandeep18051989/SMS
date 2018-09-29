using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class TemplateMap : CMSEntityTypeConfiguration<Template>
    {
        public TemplateMap()
        {
            this.ToTable("Template");
            this.HasKey(t => t.Id);

            // Relationships
            this.HasMany(t => t.Tokens).WithMany().Map(m => m.ToTable("Template_DataToken_Map").MapLeftKey("TemplateId").MapRightKey("DataTokenId"));
            EntityTracker.TrackAllProperties<Template>().Except(x => x.ModifiedOn).And(x => x.CreatedOn);
        }
    }
}
