using Marketplace.Repositories;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginViewModal request)
        {
            var customer = await _service.Login(request);
            if (customer == null)
            {
                return BadRequest("Username or password did not match.");
            }

            var accessToken = await _service.GenerateAccessToken(customer.Username, customer.RoleName);
            var response = new AuthViewModel
            {
                CustomerId = customer.Id,      
                RoleId = customer.RoleId,
                Username = customer.Username,
                Token = accessToken,
                Password = "==Secret=="
            };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(CustomerViewModel request)
        {
            var response = await _service.Register(request);
            return Ok(response);
        }

    }
}