using Marketplace.Responses;

namespace Marketplace.Services.Interface
{
    public interface IAuthService
    {
        public Task<UserViewModel> Login(LoginViewModal request);
        public Task<string> GenerateAccessToken(string username, string roleId);
        public Task<UserViewModel> Register(UserViewModel request);
        Task<bool> IsRequestPermitted();
    }

}
