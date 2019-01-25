namespace EF.Services.Social
{
    public partial interface ISocialAuthorizer
    {
        AuthorizationResult Authorize(OpenAuthenticationParameters parameters);
    }
}