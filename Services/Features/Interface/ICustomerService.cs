using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface ICustomerService : IGuidService<CustomerViewModel>
    {
        public Task<string> Login(string username, string password);
        public Task<CustomerViewModel> Register(CustomerViewModel request);
        public Task<CustomerViewModel> Edit(CustomerViewModel request);
        public Task<Guid> Delete(Guid id);
    }
}
