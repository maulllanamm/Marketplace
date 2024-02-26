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
    public class ProductService : Service<ProductViewModel, Product>, IProductService
    {
        private readonly IRepository<Product> _baseRepo;
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private readonly IDummyGenerator<ProductViewModel> _dummyGenerator;
        private static readonly Faker<ProductViewModel> ProductFaker = new Faker<ProductViewModel>("id_ID")
            .RuleFor(x => x.Name, x => x.Commerce.ProductName())
            .RuleFor(x => x.Category, x => x.Commerce.Categories(1)[0])
            .RuleFor(x => x.Price, f => f.Random.Long(100, 10000))
            .RuleFor(x => x.Description, f => f.Commerce.ProductAdjective() + " " + f.Commerce.ProductMaterial())
            .RuleFor(x => x.StockQuantity, f => f.Random.Number(1, 100));

        public ProductService(IRepository<Product> baseRepo, IProductRepository repo, IMapper mapper, IDummyGenerator<ProductViewModel> dummyGenerator) : base(mapper, baseRepo)
        {
            _baseRepo = baseRepo;
            _repo = repo;
            _mapper = mapper;
            _dummyGenerator = dummyGenerator;
        }




        public async Task<List<ProductViewModel>> GetAll()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var products = await _repo.GetAll();
            stopwatch.Stop();
            return _mapper.Map<List<ProductViewModel>>(products.Take(10));
        }

        public async Task<List<ProductViewModel>> GetProducts(string category)
        {
            var products = await _repo.GetProducts(category);
            return _mapper.Map<List<ProductViewModel>>(products);
        }
        public async Task<string> GenerateDummy(int amount)
        {
            var products = _dummyGenerator.Generate(amount, ProductFaker);
            var map = _mapper.Map<List<Product>>(products);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var entities = await _repo.CreateBulk(map);
            stopwatch.Stop();
            return $"Success generate: {entities} data , Elapsed time: {stopwatch.Elapsed}";
        }

        public async Task<string> Update(ProductViewModel product)
        {
            var map = _mapper.Map<Product>(product);
            var entities = await _repo.Update(map);
            return $"";
        }

    }
}
