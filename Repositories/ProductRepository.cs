using Marketplace.Enitities;
using Marketplace.Enitities.Base;
using Marketplace.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Interface;

namespace Marketplace.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProducts(string category)
        {
            return await _context.Products.Where(x => x.category == category).ToListAsync();
        }
       
    }
}
