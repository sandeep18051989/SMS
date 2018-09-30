using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
    public partial class ScheduleTaskMap : CMSEntityTypeConfiguration<ScheduleTask>
    {
        public ScheduleTaskMap()
        {
            this.ToTable("ScheduleTask");
            this.HasKey(t => t.Id);
            this.Property(t => t.Name).IsRequired();
            this.Property(t => t.Type).IsRequired();
        }
    }
}