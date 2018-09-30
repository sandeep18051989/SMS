using EF.Core.Data;
using EF.Data.Configuration;
using System.Data.Entity.ModelConfiguration;

namespace EF.Data.Mapping
{
    public partial class SystemLogMap : CMSEntityTypeConfiguration<SystemLog>
    {
        public SystemLogMap()
        {
            this.ToTable("SystemLog");
            this.HasKey(sl => sl.Id);
            this.Property(sl => sl.EntityTypeName).HasMaxLength(400);

        }
    }
}
