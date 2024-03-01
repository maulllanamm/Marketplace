using Marketplace.Enitities;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface IShoppingCartService : IService<ShoppingCartViewModel>
    {
        public Task<ShoppingCartViewModel> AddProduct(ItemCartViewModel item);
        public Task<List<ShoppingCartViewModel>> GetListShoppingCart();
    }
}
