using System.Collections.Generic;
namespace EF.Core.Data
{
    public class SocialSettings
    {
        public SocialSettings()
        {
            ActiveAuthenticationMethodSystemNames = new List<string>();
        }

        public bool AutoRegisterEnabled { get; set; }

        public bool RequireEmailValidation { get; set; }

        public List<string> ActiveAuthenticationMethodSystemNames { get; set; }
    }
}