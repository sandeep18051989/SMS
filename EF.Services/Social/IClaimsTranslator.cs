namespace EF.Services.Social
{
    public partial interface IClaimsTranslator<T>
    {
        UserClaims Translate(T response);
    }
}