using EF.Core;

namespace EF.Facebook
{
    public class FacebookSocialAuthSettings : ISettings
    {
        public string ClientKeyIdentifier { get; set; }
        public string ClientSecret { get; set; }
    }
}
