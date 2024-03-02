using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;
using System.Data.Entity.Core.Objects.DataClasses;

namespace Marketplace.Requests
{
    public class OrderService : Service<OrderViewModel, Order>, IOrderService
    {
        private readonly IRepository<Order> _baseRepo;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderItemService _orderItemService;
        private readonly IShoppingCartService _shoppingCartService;
        public OrderService(IRepository<Order> baseRepo, IMapper mapper, IAuthService auth, IUserService userService, IOrderItemService orderItemService, IProductService productService, IShoppingCartService shoppingCartService) : base(mapper, baseRepo)
        {
            _baseRepo = baseRepo;
            _mapper = mapper;
            _authService = auth;
            _userService = userService;
            _orderItemService = orderItemService;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }


        public async Task<OrderViewModel> CheckOut(List<int> shoppingCartsId)
        {
            var listShoppingCart = await _shoppingCartService.GetByListId(shoppingCartsId);
            var getMe = await _authService.GetMe();
            var user = await _userService.GetByUsername(getMe.Username);
            var listProductId = listShoppingCart.Select(x => x.ProductId).ToList();
            var products = await _productService.GetByListId(listProductId);
            var order = new OrderViewModel
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = user.Address,
                TotalPrice = listShoppingCart.Sum(x => x.TotalPrice),
                CreatedBy = user.Username,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = user.Username,
                ModifiedDate = DateTime.UtcNow,

            };
            var newOrder = await _baseRepo.Create(_mapper.Map<Order>(order));

            var orderItems = new List<OrderItemViewModel>();
            var productUpdated = new List<ProductViewModel>();

            foreach (var orderItem in listShoppingCart)
            {
                orderItems.Add(new OrderItemViewModel
                {
                    OrderId = newOrder.id,
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity,
                    UnitPrice = orderItem.TotalPrice / orderItem.Quantity,
                    TotalPrice = orderItem.TotalPrice,
                    CreatedBy = user.Username,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = user.Username,
                    ModifiedDate = DateTime.UtcNow,
                });

                var product = productUpdated.FirstOrDefault(x => x.Id == orderItem.ProductId);
                product.StockQuantity -= orderItem.Quantity;
                productUpdated.Add(product);
            }


            await _orderItemService.CreateBulk(orderItems);
            await _productService.UpdateBulk(productUpdated);
            await _shoppingCartService.SoftDeleteBulk(listShoppingCart);
            return _mapper.Map<OrderViewModel>(newOrder);

        }
    }
}
