using AutoMapper;
using Bogus;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;
using Repositories.Interface;
using System.Diagnostics;
using Z.BulkOperations;

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

        public async Task<List<ShoppingCartViewModel>> GetListShoppingCart()
        {
            var userLogin = await _auth.GetMe();
            var allShoppingCart = await _baseRepo.GetAll();
            var myShoppingCart = allShoppingCart.Where(x => x.created_by == userLogin.Username);
            return _mapper.Map<List<ShoppingCartViewModel>>(myShoppingCart);
        }
        public async Task<ShoppingCartViewModel> AddProduct(ItemCartViewModel item)
        {
            var userLogin = await _auth.GetMe();
            var user = await _users.GetByUsername(userLogin.Username);
            var product = await _product.GetById(item.ProductId);
            if(product.StockQuantity < item.Quantity)
            {
                throw new Exception("Stok produk tidak mencukupi untuk jumlah yang diminta.");
            }
            var shoppingCart = new ShoppingCart
            {
                product_id = item.ProductId,
                user_id = user.Id,
                quantity = item.Quantity,
                total_price = item.Quantity * product.Price,
                created_by = userLogin.Username
            };
            await _baseRepo.Create(shoppingCart);

            return _mapper.Map<ShoppingCartViewModel>(shoppingCart);
        }
        public override async Task<int> Delete(int id)
        {
            var userLogin = await _auth.GetMe();
            var allShoppingCart = await _baseRepo.GetAll();
            var myShoppingCart = allShoppingCart.FirstOrDefault(x => x.created_by == userLogin.Username && x.id == id);
            if(myShoppingCart != null)
            {
                await _baseRepo.Delete(id);
            }
            return id;
        }
    }
}
