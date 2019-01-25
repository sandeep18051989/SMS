using EF.Core.Social;
namespace EF.Data.Configuration
{
    public partial class SocialSettingMap : CMSEntityTypeConfiguration<SocialSetting>
    {
        public SocialSettingMap()
        {
            this.ToTable("SocialSetting");
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired().HasMaxLength(200);
            this.Property(s => s.Value).IsRequired().HasMaxLength(2000);
        }
    }
}