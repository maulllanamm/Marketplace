using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface IProductService : IService<ProductViewModel>
    {
        public Task<List<ProductViewModel>> GetAll();
        public Task<List<ProductViewModel>> GetProducts(string category);
        public Task<string> GenerateDummy(int amount);
        public Task<string> Update(ProductViewModel product);
    }
}
