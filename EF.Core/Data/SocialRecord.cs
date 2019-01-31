namespace EF.Core.Data
{
    public partial class SocialRecord : BaseEntity
    {
        public string Email { get; set; }

        public string Identifier { get; set; }

        public string DisplayIdentifier { get; set; }

        public string OAuthToken { get; set; }

        public string OAuthAccessToken { get; set; }

        public string ProviderSystemName { get; set; }

        public virtual User User { get; set; }
    }

}
