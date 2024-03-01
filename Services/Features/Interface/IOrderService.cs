using Marketplace.Enitities;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface IOrderService : IService<OrderViewModel>
    {
        public Task<OrderViewModel> CheckOut(List<int> shoppingCartsId);
    }
}
