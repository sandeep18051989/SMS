using EF.Core.Data;
using System;
namespace EF.Services.Social
{
    public static class OpenAuthenticationExtensions
    {
        public static bool IsMethodActive(this ISocialAuthenticationMethod method,
            SocialSettings settings)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (settings == null)
                throw new ArgumentNullException("settings");

            if (settings.ActiveAuthenticationMethodSystemNames == null)
                return false;
            foreach (string activeMethodSystemName in settings.ActiveAuthenticationMethodSystemNames)
                if (method.IsMethodActive(settings))
                    return true;
            return false;
        }
    }
}
