using Marketplace.Repositories;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetMe()
        {
            var user = await _service.GetMe();
            return Ok(user);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginViewModal request)
        {
            var user = await _service.Login(request);
            if (user == null)
            {
                return BadRequest("Username or password did not match.");
            }

            var accessToken = await _service.GenerateAccessToken(user.Username, user.RoleName);
            return Ok(accessToken);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(UserViewModel request)
        {
            var response = await _service.Register(request);
            return Ok(response);
        }

    }
}