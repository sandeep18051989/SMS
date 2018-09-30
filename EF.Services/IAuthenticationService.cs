using EF.Core.Data;

namespace EF.Services
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService 
    {
        void SignIn(User user, bool createPersistentCookie);
        void SignOut();
		User GetAuthenticatedUser();
    }
}