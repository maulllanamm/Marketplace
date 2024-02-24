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

        [HttpGet]
        public async Task<ActionResult> Login(string username, string password)
        {
            var customer = await _customerService.Login(username, password);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult> Register(CustomerViewModel request)
        {
            var customer = await _customerService.Register(request);
            return Ok(customer);
        }

        [HttpPut]
        public async Task<ActionResult> Put(CustomerViewModel request)
        {
            var customer = await _customerService.Edit(request);
            return Ok(customer);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            var customer =  await _customerService.Delete(id);
            return Ok(customer);
        }
    }
}