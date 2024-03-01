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
        private readonly IAuthService _auth;
        private readonly IProductService _product;
        private readonly IUserService _users;
        private readonly IShoppingCartRepository _repo;
        private readonly IMapper _mapper;
        public ShoppingCartService(IRepository<ShoppingCart> baseRepo, IShoppingCartRepository repo, IMapper mapper, IAuthService auth, IProductService product, IUserService users) : base(mapper, baseRepo)
        {
            _baseRepo = baseRepo;
            _repo = repo;
            _mapper = mapper;
            _auth = auth;
            _product = product;
            _users = users;
        }

        public async Task<ShoppingCartViewModel> AddProduct(ItemCartViewModel item)
        {
            var userLogin = await _auth.GetMe();
            var user = await _users.GetByUsername(userLogin.Username);
            var product = await _product.GetById(item.ProductId);
            var shoppingCart = new ShoppingCart
            {
                product_id = item.ProductId,
                user_id = user.Id,
                quantity = item.Quantity,
                total_amount = item.Quantity * product.Price,
                created_by = userLogin.Username
            };
            await _baseRepo.Create(shoppingCart);

            product.StockQuantity -= item.Quantity;

            await _product.Update(product);
            return _mapper.Map<ShoppingCartViewModel>(shoppingCart);
        }

    }
}
