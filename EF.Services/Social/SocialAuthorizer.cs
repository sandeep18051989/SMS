using System;
using EF.Services.Service;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;

namespace EF.Services.Social
{
    public partial class SocialAuthorizer : ISocialAuthorizer
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IUserContext _userContext;
        private readonly SocialSettings _socialSettings;
        #endregion

        #region Ctor

        public SocialAuthorizer(IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService,
            IUserService userService,
            IUserContext userContext,
            SocialSettings socialSettings,
            IEmailService emailService)
        {
            this._authenticationService = authenticationService;
            this._openAuthenticationService = openAuthenticationService;
            this._userService = userService;
            this._userContext = userContext;
            this._socialSettings = socialSettings;
            this._emailService = emailService;
        }

        #endregion

        #region Utilities

        private bool AccountDoesNotExistAndUserIsNotLoggedOn(User userFound, User userLoggedIn)
        {
            return userFound == null && userLoggedIn == null;
        }

        private bool AccountIsAssignedToLoggedOnAccount(User userFound, User userLoggedIn)
        {
            return userFound.Id.Equals(userLoggedIn.Id);
        }

        private bool AccountAlreadyExists(User userFound, User userLoggedIn)
        {
            return userFound != null && userLoggedIn != null;
        }

        #endregion

        #region Methods

        public virtual AuthorizationResult Authorize(OpenAuthenticationParameters parameters)
        {
            var userFound = _openAuthenticationService.GetUser(parameters);

            var userLoggedIn = _userContext.CurrentUser;

            if (AccountAlreadyExists(userFound, userLoggedIn))
            {
                if (AccountIsAssignedToLoggedOnAccount(userFound, userLoggedIn))
                {
                    return new AuthorizationResult(OpenAuthenticationStatus.Authenticated);
                }

                var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                result.AddError("Account is already assigned");
                return result;
            }
            if (AccountDoesNotExistAndUserIsNotLoggedOn(userFound, userLoggedIn))
            {
                SocialAuthorizerHelper.StoreParametersForRoundTrip(parameters);

                #region Register user

                var currentUser = _userContext.CurrentUser;
                var details = new RegistrationDetails(parameters);
                var randomPassword = CommonHelper.GenerateRandomDigitCode(20);

                bool isApproved = !_socialSettings.RequireEmailValidation;

                var registrationRequest = new UserRegistrationRequest(currentUser,
                    details.EmailAddress,
                    details.UserName,
                    randomPassword,
                    isApproved);

                var registrationResult = _userService.RegisterUser(registrationRequest);
                if (registrationResult.Success)
                {
                    userFound = currentUser;
                    _openAuthenticationService.AssociateSocialAccountWithUser(currentUser, parameters);
                    SocialAuthorizerHelper.RemoveParameters();

                    //authenticate
                    if (isApproved)
                        _authenticationService.SignIn(userFound ?? userLoggedIn, false);

                    if (isApproved)
                    {
                        //send user welcome message
                        _emailService.SendUserWelcomeMessage(currentUser);

                        //result
                        return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredStandard);
                    }
                }
                else
                {
                    SocialAuthorizerHelper.RemoveParameters();

                    var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                    foreach (var error in registrationResult.Errors)
                        result.AddError(string.Format(error));
                    return result;
                }

                #endregion
            }
            if (userFound == null)
            {
                _openAuthenticationService.AssociateSocialAccountWithUser(userLoggedIn, parameters);
            }

            //authenticate
            _authenticationService.SignIn(userFound ?? userLoggedIn, false);
            return new AuthorizationResult(OpenAuthenticationStatus.Authenticated);
        }

        #endregion
    }
}