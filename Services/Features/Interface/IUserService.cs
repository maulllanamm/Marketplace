using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface IUserService : IGuidService<UserViewModel>
    {
        public Task<string> Login(string username, string password);
        public Task<UserViewModel> GetByUsername(string username);
        public Task<UserViewModel> Register(UserViewModel request);
        public Task<UserViewModel> Update(UserViewModel request);
        public Task<Guid> Delete(Guid id);
    }
}
