using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
    public partial class SocialProviderMap : CMSEntityTypeConfiguration<SocialProvider>
    {
        public SocialProviderMap()
        {
            this.ToTable("SocialProvider");
            this.HasKey(ear => ear.Id);
            this.Property(ear => ear.ProviderSystemName).IsRequired();
            this.Property(ear => ear.DisplayIdentifier).IsOptional();
            this.Property(ear => ear.AuthenticationMethodService).IsRequired();
            this.Property(ear => ear.AuthenticationMethodServiceNamespace).IsRequired();
            this.Property(ear => ear.Version).IsOptional();
        }
    }
}