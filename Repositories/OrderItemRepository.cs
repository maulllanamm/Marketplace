using Marketplace.Enitities;
using Marketplace.Repositories.Base;
using Repositories.Interface;

namespace Marketplace.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly DataContext _context;
        public OrderItemRepository(DataContext context) : base(context)
        {
            _context = context;
        }

       
    }
}
