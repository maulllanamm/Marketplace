using AutoMapper;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Marketplace.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController : BaseGuidController<CustomerViewModel>
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService) : base(customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public override async Task<ActionResult> Post(CustomerViewModel request)
        {
            var customer = _customerService.Register(request);
            return Ok(customer);
        }
    }
}