using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Constants;

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [HttpGet, Authorize(Roles = "1")]
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


        [HttpPut]
        public async Task<ActionResult> Put(ProductViewModel product)
        {
            var customer = await _productService.Update(product);
            return Ok(customer);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var customer = await _productService.Delete(id);
            return Ok(customer);
        }
    }
}