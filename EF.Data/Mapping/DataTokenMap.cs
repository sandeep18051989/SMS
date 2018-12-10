using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class DataTokenMap : CMSEntityTypeConfiguration<DataToken>
    {
        public DataTokenMap()
        {
            this.ToTable("DataToken");
            this.HasKey(dt => dt.Id);
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.SystemName).IsOptional();
            this.Property(b => b.Value).IsRequired();

            // Relationships
            this.HasRequired(cust => cust.user)
                .WithMany()
                .HasForeignKey(dt => dt.UserId);

            EntityTracker.TrackAllProperties<DataToken>().Except(x => x.user).And(x => x.ModifiedOn).And(x => x.CreatedOn).And(x => x.user).And(x => x.UserId);

        }
    }
}
