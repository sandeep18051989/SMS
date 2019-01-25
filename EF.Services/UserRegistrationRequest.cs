using EF.Core.Data;

namespace EF.Services
{
    public class UserRegistrationRequest
    {

        public UserRegistrationRequest(User customer, string email, string username,
            string password,
            bool isApproved = true,
            bool isActive = true)
        {
            this.User = customer;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.IsApproved = isApproved;
            this.IsActive = isActive;
        }

        public User User { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
    }
}
