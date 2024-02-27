using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Interface;
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

        //[HttpGet]
        //public async Task<ActionResult> AddProduct(ShoppingCartViewModel )
        //{
        //    var products = await _shoppingCartService.AddProduct();
        //    return Ok(products);
        //}

        
    }
}