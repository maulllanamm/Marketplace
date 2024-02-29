using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ViewModels.ants;

namespace Marketplace.Requests
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtModel _jwt;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public JwtMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, IOptions<JwtModel> jwt)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _jwt = jwt.Value;
        }

        public async Task<bool> InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                // Gunakan authService di sini
                if (await authService.IsRequestPermitted())
                {
                    await _next(context);
                    return true;
                }
                else//NOT PERMITTED
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }
            }
        }

        
    }

    public static class SwaggerAuthorizeExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
