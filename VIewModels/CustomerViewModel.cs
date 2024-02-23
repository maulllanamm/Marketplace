using Marketplace.Responses.Base;

namespace Marketplace.Responses
{
    public class CustomerViewModel : GuidViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }


}
