using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts(string category)
        {
            var products = await _productService.GetProducts(category);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateDummy(int amount)
        {
            var products = await _productService.GenerateDummy(amount);
            return Ok(products);
        }

    }
}