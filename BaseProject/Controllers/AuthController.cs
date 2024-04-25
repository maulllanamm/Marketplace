using Marketplace.Repositories;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IAuthService service, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetMe()
        {
            var user = await _service.GetMe();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> RefreshToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Invalid token.");
            }

            var principal = _service.ValidateAccessToken(refreshToken);
            if (principal == null)
            {
                return Unauthorized("Invalid token.");
            }

            // Mendapatkan informasi user dari token
            var username = principal.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetByUsername(username);
            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }
            string newToken = await _service.GenerateAccessToken(user.Username, user.RoleName);
            var newRefreshToken = await _service.GenerateRefreshToken(user.Username);
            _service.SetRefreshToken(newRefreshToken, user);
            return Ok(newToken);
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
            var refreshToken = await _service.GenerateRefreshToken(user.Username);
            _service.SetRefreshToken(refreshToken, user);
            return Ok(accessToken);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModal request)
        {
            var response = await _service.Register(request);
            return Ok(response);
        }

    }
}