using Marketplace.Enitities;
using Marketplace.Responses;
using System.Security.Claims;
using ViewModels.ants;

namespace Marketplace.Services.Interface
{
    public interface IAuthService
    {
        public Task<UserViewModel> Login(LoginViewModal request);
        public Task<GetMeViewModal> GetMe();
        public Task<string> GenerateAccessToken(string username, string roleId);
        public Task<string> GenerateRefreshToken(string username);
        public void SetRefreshToken(string newRefreshToken, UserViewModel user);
        public Task<UserViewModel> Register(RegisterViewModal request);
        public Task<bool> IsRequestPermitted();
        public ClaimsPrincipal ValidateAccessToken(string token);
    }

}
