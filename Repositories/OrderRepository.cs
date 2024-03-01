using Marketplace.Enitities;
using Marketplace.Repositories.Base;
using Repositories.Interface;

namespace Marketplace.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        public OrderRepository(DataContext context) : base(context)
        {
            _context = context;
        }

       
    }
}
