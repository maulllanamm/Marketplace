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
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var claims = ValidateAccessToken(token);
                context.User = claims;
            }
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

        private ClaimsPrincipal ValidateAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));

                tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = key,
                    ValidIssuer = _jwt.Issuer,
                    ValidAudience = _jwt.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);


                var jsonToken = validatedToken as JwtSecurityToken;

                if (jsonToken != null)
                {
                    var claimsIdentity = new ClaimsIdentity(jsonToken.Claims, "jwt");

                    // Set the claims to context.User
                    return new ClaimsPrincipal(claimsIdentity);
                }

                return new ClaimsPrincipal();
            }
            catch
            {
                return null;
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
