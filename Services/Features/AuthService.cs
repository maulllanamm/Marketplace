using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.Base;
using Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ViewModels.ants;

namespace Marketplace.Requests
{
    public class AuthService : IAuthService
    {
        private readonly IGuidRepository<User> _baseRepo;
        private readonly IUserRepository _userRepo;
        private readonly IRoleService _role;
        private readonly IPermissionService _permission;
        private readonly IRolePermissionService _rolePermission;
        private readonly IHttpContextAccessor _httpCont;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly JwtModel _jwt;
        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;

        public AuthService(IGuidRepository<User> baseRepo, IUserRepository repo,
            IRoleService role, IPermissionService permission, IHttpContextAccessor httpCont, IPasswordHasher passwordHasher, IMapper mapper, IOptions<JwtModel> jwt,
            IRolePermissionService rolePermission)
        {
            _baseRepo = baseRepo;
            _userRepo = repo;
            _role = role;
            _permission = permission;
            _httpCont = httpCont;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _jwt = jwt.Value;
            _rolePermission = rolePermission;
        }

        public Task<GetMeViewModal> GetMe()
        {
            if (_httpCont.HttpContext != null)
            {
                var username = _httpCont.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var role = _httpCont.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var result = new GetMeViewModal
                {
                    Username = username,
                    Role = role,
                };

                return Task.FromResult<GetMeViewModal>(result);
            }

            return Task.FromResult<GetMeViewModal>(null);
        }


        public async Task<UserViewModel> Login(LoginViewModal request)
        {
            var user = await _userRepo.GetByUsername(request.Username);
            if (user == null)
            {
                return null;
            }

            var passwordHash = _passwordHasher.ComputeHash(request.Password, user.password_salt, _papper, _iteration);
            if (passwordHash != user.password_hash)
            {
                return null;
            }
            return _mapper.Map<UserViewModel>(user);
        }
        public async Task<string> GenerateRefreshToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };

            var tokenDescriptor = new JwtSecurityToken
                (
                    _jwt.Issuer,
                    _jwt.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryRefreshMinutes),
                    signingCredentials: credential
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenDescriptor);
        }

        public void SetRefreshToken(string newRefreshToken, UserViewModel user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryRefreshMinutes)
            };
            _httpCont.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

            user.RefreshToken = newRefreshToken;
            user.TokenCreated = DateTime.UtcNow;
            user.TokenExpires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryRefreshMinutes);

            _baseRepo.Update(_mapper.Map<User>(user));
        }

        public async Task<string> GenerateAccessToken(string username, string roleName)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, roleName.ToString()),
            };

            var tokenDescriptor = new JwtSecurityToken
                (
                    _jwt.Issuer,
                    _jwt.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryAccessMinutes),
                    signingCredentials: credential
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenDescriptor);
        }
        public async Task<UserViewModel> Register(RegisterViewModal request)
        {
            var role = await _role.GetById(request.RoleId);
            var user = new User
            {
                role_id = role.Id,
                role_name = role.Name,
                username = request.Username,
                email = request.Email,
                password_salt = _passwordHasher.GenerateSalt(),
                full_name = request.FullName,
                phone_number = request.PhoneNumber,
                address = request.Address
            };

            user.password_hash = _passwordHasher.ComputeHash(request.Password, user.password_salt, _papper, _iteration);
            _baseRepo.Create(user);
            var res = _mapper.Map<UserViewModel>(user);
            res.Password = "==HASH==";
            return res;
        }

        public async Task<bool> IsRequestPermitted()
        {
            var getMe = await GetMe();
            var username = getMe.Username;
            var role = getMe.Role;
            var path = _httpCont.HttpContext.Request.Path.Value;
            var method = _httpCont.HttpContext.Request.Method.ToString();

            if (role == "Administrator")
            {
                return true;
            }
            var roles = await _role.GetAll();
            var permissions = await _permission.GetAll();
            var rolePermissions = await _rolePermission.GetAll();

            var myRole = roles.FirstOrDefault(x => x.Name == role);
            if(myRole == null)
            {
                return true;
            }
            var myRolePermissions = rolePermissions.Where(x => x.role_id == myRole.Id).Select(x => x.permission_id).ToList();
            var myPermissions = permissions.Where(x => myRolePermissions.Contains(x.Id));

            var checkPermission = myPermissions.FirstOrDefault(x => x.HttpMethod == method && x.Path == path);
            if (checkPermission == null)
            {
                return false;
            }
            return true;

        }

        public ClaimsPrincipal ValidateAccessToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
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

                var isValidToken = jsonToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase);
                if (jsonToken == null || !isValidToken)
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
