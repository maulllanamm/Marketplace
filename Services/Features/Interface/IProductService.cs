using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface IProductService : IService<ProductViewModel>
    {
        public Task<List<ProductViewModel>> GetProducts(string category);
        public Task<List<ProductViewModel>> GenerateDummy(int amount);
    }
}
