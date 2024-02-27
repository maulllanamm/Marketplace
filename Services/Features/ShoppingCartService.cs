using AutoMapper;
using Bogus;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;
using Repositories.Interface;
using System.Diagnostics;

namespace Marketplace.Requests
{
    public class ShoppingCartService : Service<ShoppingCartViewModel, ShoppingCart>, IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _baseRepo;
        private readonly IShoppingCartRepository _repo;
        private readonly IMapper _mapper;
        public ShoppingCartService(IRepository<ShoppingCart> baseRepo, IShoppingCartRepository repo, IMapper mapper) : base(mapper, baseRepo)
        {
            _baseRepo = baseRepo;
            _repo = repo;
            _mapper = mapper;
        }

        //public async Task<List<ProductViewModel>> GetProducts(string category)
        //{
        //    var products = await _repo.GetProducts(category);
        //    return _mapper.Map<List<ProductViewModel>>(products);
        //}

    }
}
