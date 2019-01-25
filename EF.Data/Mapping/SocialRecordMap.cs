using EF.Core.Data;
using EF.Data.Configuration;

namespace EF.Data.Mapping
{
    public partial class SocialRecordMap : CMSEntityTypeConfiguration<SocialRecord>
    {
        public SocialRecordMap()
        {
            this.ToTable("SocialRecord");
            this.HasKey(ear => ear.Id);
            this.Property(ear => ear.Email).IsRequired();
            this.Property(ear => ear.DisplayIdentifier).IsOptional();
            this.Property(ear => ear.Identifier).IsOptional();
            this.Property(ear => ear.OAuthAccessToken).IsOptional();
            this.Property(ear => ear.OAuthToken).IsOptional();
            this.Property(ear => ear.ProviderSystemName).IsOptional();

            this.HasRequired(ear => ear.User)
                .WithMany(c => c.SocialRecords)
                .HasForeignKey(ear => ear.UserId);

        }
    }
}