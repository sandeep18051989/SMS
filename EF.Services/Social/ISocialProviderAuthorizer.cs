namespace EF.Services.Social
{
    public partial interface ISocialProviderAuthorizer
    {
        AuthorizeState Authorize(string returnUrl, bool? verifyResponse = null);
    }
}