using EF.Core.Data;
using System;
using System.Collections.Generic;
namespace EF.Services.Social
{
    public partial interface IOpenAuthenticationService
    {
        #region Social authentication methods

        IList<SocialProvider> LoadActiveSocialAuthenticationMethods(bool? onlyActive = null);

        #endregion

        void AssociateSocialAccountWithUser(User user, OpenAuthenticationParameters parameters);

        bool AccountExists(OpenAuthenticationParameters parameters);

        User GetUser(OpenAuthenticationParameters parameters);

        IList<SocialRecord> GetSocialIdentifiersFor(User user);

        void DeleteSocialRecord(SocialRecord externalAuthenticationRecord);

        void RemoveAssociation(OpenAuthenticationParameters parameters);
    }
}