using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetListShoppingCart()
        {
            var products = await _shoppingCartService.GetListShoppingCart();
            return Ok(products);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddProduct(ItemCartViewModel item)
        {
            var products = await _shoppingCartService.AddProduct(item);
            return Ok(products);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var products = await _shoppingCartService.Delete(id);
            return Ok(products);
        }
    }
}