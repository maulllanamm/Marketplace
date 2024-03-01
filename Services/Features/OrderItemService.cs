using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;

namespace Marketplace.Requests
{
    public class OrderItemService : Service<OrderItemViewModel, OrderItem>, IOrderItemService
    {
        private readonly IRepository<OrderItem> _baseRepo;
        private readonly IMapper _mapper;
        public OrderItemService(IRepository<OrderItem> baseRepo, IMapper mapper) : base(mapper, baseRepo)
        {
            _baseRepo = baseRepo;
            _mapper = mapper;
        }

    }
}
