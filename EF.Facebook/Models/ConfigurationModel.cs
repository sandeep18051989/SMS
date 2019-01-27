using EF.Services;

namespace EF.Facebook.Models
{
    public class ConfigurationModel : BaseEntityModel
    {
        public string ClientKeyIdentifier { get; set; }
        public string ClientSecret { get; set; }

        public string DisplayName { get; set; }
    }
}