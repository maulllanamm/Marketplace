using Marketplace.Responses;

namespace Marketplace.Services.Interface
{
    public interface IAuthService
    {
        public Task<CustomerViewModel> Login(LoginViewModal request);
        public Task<string> GenerateAccessToken(Guid customerId, int roleId);
        public Task<CustomerViewModel> Register(CustomerViewModel request);
    }

}
