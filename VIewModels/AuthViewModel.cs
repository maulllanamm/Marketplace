using Marketplace.Responses.Base;

namespace Marketplace.Responses
{
    public class AuthViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
    }


    public class LoginViewModal
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }    
    
    public class RegisterViewModal
    {
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class GetMeViewModal
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }

}
