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
        public async Task<ProductResponseEntitiy> GetAll(int page)
        {
            var pageResult = 5f;
            var countProduct = await _context.Products.CountAsync() ;
            var totalPage = Math.Ceiling(countProduct / pageResult);

            var products = await _context.Products
                                        .Skip((page - 1) * (int)pageResult)
                                        .Take((int)pageResult)
                                        .ToListAsync();
            return new ProductResponseEntitiy
            {
                Products = products,
                CurrentPage = page,
                TotalPage = (int)totalPage
            };
        }
        public async Task<List<Product>> GetProducts(string category)
        {
            return await _context.Products.Where(x => x.category == category).ToListAsync();
        }
       
    }
}
