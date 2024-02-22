using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface ICustomerService : IBaseGuidService<CustomerViewModel>
    {
        public Task<CustomerViewModel> Register(CustomerViewModel request);
    }
}
