using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class PermissionRecordMap : CMSEntityTypeConfiguration<PermissionRecord>
    {
        public PermissionRecordMap()
        {
            this.ToTable("Permission");
            this.HasKey(per => per.Id);
            this.Property(b => b.Name).IsRequired();
            this.Property(b => b.SystemName).IsOptional();
            EntityTracker.TrackAllProperties<PermissionRecord>().Except(x => x.IsSystemDefined).And(x => x.ModifiedOn).And(x => x.CreatedOn);
        }
    }
}
