using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService) 
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CustomerViewModel request)
        {
            var customer = _customerService.Register(request);
            return Ok(customer);
        }
    }
}