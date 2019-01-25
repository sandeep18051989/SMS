using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Services.Service;
using EF.Core.Data;
using EF.Core.Social;
using System.Reflection;

namespace EF.Services.Social
{
    public partial class OpenAuthenticationService : IOpenAuthenticationService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly ISocialPluginService _socialPluginService;
        private readonly IRepository<SocialRecord> _socialRecordRepository;

        #endregion

        #region Ctor

        public OpenAuthenticationService(IRepository<SocialRecord> socialRecordRepository,
            IUserService userService,
            ISocialPluginService socialPluginService)
        {
            this._socialRecordRepository = socialRecordRepository;
            this._userService = userService;
            this._socialPluginService = socialPluginService;
        }

        #endregion

        #region Methods

        #region Social authentication methods

        public virtual IList<Type> LoadActiveSocialAuthenticationMethods(User user = null, bool? onlyActive=null) // ISocialAuthenticationMethod
        {
            return LoadAllSocialAuthenticationMethods(user, onlyActive).Where(provider => Assembly.GetExecutingAssembly().GetTypes().Any(type => !String.IsNullOrEmpty(type.Namespace) && type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(ISocial))).ToList();
        }

        public virtual IList<Type> LoadAllSocialAuthenticationMethods(User user = null, bool? onlyActive=null)
        {
            return _socialPluginService.GetSocialPlugins<ISocial>(user: user, onlyActive: onlyActive).ToList();
        }

        #endregion

        public virtual void AssociateSocialAccountWithUser(User user, OpenAuthenticationParameters parameters)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            string email = null;
            if (parameters.UserClaims != null)
                foreach (var userClaim in parameters.UserClaims.Where(x => x.Contact != null && !String.IsNullOrEmpty(x.Contact.Email)))
                {
                    email = userClaim.Contact.Email;
                    break;
                }

            var socialRecord = new SocialRecord
            {
                UserId = user.Id,
                Email = email,
                Identifier = parameters.Identifier,
                DisplayIdentifier = parameters.DisplayIdentifier,
                OAuthToken = parameters.OAuthToken,
                OAuthAccessToken = parameters.OAuthAccessToken,
                ProviderSystemName = parameters.ProviderSystemName,
            };

            _socialRecordRepository.Insert(socialRecord);
        }

        public virtual bool AccountExists(OpenAuthenticationParameters parameters)
        {
            return GetUser(parameters) != null;
        }

        public virtual User GetUser(OpenAuthenticationParameters parameters)
        {
            var record = _socialRecordRepository.Table
                .FirstOrDefault(o => o.Identifier == parameters.Identifier &&
                    o.ProviderSystemName == parameters.ProviderSystemName);

            if (record != null)
                return _userService.GetUserById(record.UserId);

            return null;
        }

        public virtual IList<SocialRecord> GetSocialIdentifiersFor(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return user.SocialRecords.ToList();
        }

        public virtual void DeleteSocialRecord(SocialRecord socialRecord)
        {
            if (socialRecord == null)
                throw new ArgumentNullException("socialRecord");

            _socialRecordRepository.Delete(socialRecord);
        }

        public virtual void RemoveAssociation(OpenAuthenticationParameters parameters)
        {
            var record = _socialRecordRepository.Table
                .FirstOrDefault(o => o.Identifier == parameters.Identifier &&
                    o.ProviderSystemName == parameters.ProviderSystemName);

            if (record != null)
                _socialRecordRepository.Delete(record);
        }

        #endregion
    }
}