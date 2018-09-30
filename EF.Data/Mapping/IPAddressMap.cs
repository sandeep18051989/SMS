using EF.Core.Data;
using EF.Data.Configuration;
using TrackerEnabledDbContext.Common.Configuration;

namespace EF.Data.Mapping
{
    public partial class IPAddressMap : CMSEntityTypeConfiguration<Location>
    {
        public IPAddressMap()
        {
            this.ToTable("IPAddress");
            this.HasKey(ip => ip.Id);

            //relationship  
            EntityTracker.TrackAllProperties<Location>().Except(x => x.User).And(x => x.ModifiedOn).And(x => x.CreatedOn);
        }
    }
}
