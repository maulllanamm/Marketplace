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
        [HttpPost]
        public async Task<ActionResult> AddProduct(ItemCartViewModel item)
        {
            var products = await _shoppingCartService.AddProduct(item);
            return Ok(products);
        }


    }
}