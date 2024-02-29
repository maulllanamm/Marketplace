using FluentValidation;
using FluentValidation.Results;
using Marketplace.Responses;
using Marketplace.Responses.Base;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<int> _validator;
        public ProductController(IProductService productService, IValidator<int> validator)
        {
            _productService = productService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(int page)
        {
            ValidationResult validationResult = _validator.Validate(page);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var pageResult = 5f;
            var countProduct = await _productService.Count();
            var totalPage = Math.Ceiling(countProduct / pageResult);
            var pages = await _productService.GetAll(page);

            var products = new ProductPagination
            {
                Products = pages,
                CurrentPage = page,
                TotalPage = (int)totalPage
            };
            return Ok(products);
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts(string category)
        {
            var products = await _productService.GetProducts(category);
            return Ok(products);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> GenerateDummy(int amount)
        {
            var products = await _productService.GenerateDummy(amount);
            return Ok(products);
        }


        [HttpPut]
        public async Task<ActionResult> Put(ProductViewModel product)
        {
            var res = await _productService.Update(product);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var res = await _productService.Delete(id);
            return Ok(res);
        }
    }
}