
namespace EF.Facebook.Models
{
    public class LoginModel
    {
        public string SocialIdentifier { get; set; }
        public string KnownProvider { get; set; }
        public string ReturnUrl { get; set; }
    }
}