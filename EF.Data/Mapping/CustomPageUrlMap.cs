using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
    public partial class CustomPageUrlMap : CMSEntityTypeConfiguration<CustomPageUrl>
    {
        public CustomPageUrlMap()
        {
            this.ToTable("CustomPageUrl");
            this.HasKey(lp => lp.Id);

            this.Property(lp => lp.EntityName).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.Slug).IsRequired().HasMaxLength(400);
        }
    }
}