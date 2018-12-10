using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class SetingsMap : CMSEntityTypeConfiguration<Settings>
    {
        public SetingsMap()
        {
            this.ToTable("Settings");
            this.HasKey(s => s.Id);
            this.Property(b => b.EntityId).IsOptional();
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.SettingType).IsOptional();
            this.Property(b => b.TypeId).IsOptional();
            this.Property(b => b.Value).IsRequired();

            // Relationships
            this.HasRequired(cust => cust.user)
                .WithMany()
                .HasForeignKey(cust => cust.UserId);

            EntityTracker.TrackAllProperties<Settings>().Except(x => x.user).And(x => x.ModifiedOn).And(x => x.CreatedOn);

        }
    }
}
