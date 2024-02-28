using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.Base;
using Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using ViewModels.ants;
using ViewModels.Constants;

namespace Marketplace.Requests
{
    public class AuthService : IAuthService
    {
        private readonly IGuidRepository<Customer> _baseRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly ICustomerService _customer;
        private readonly IRoleService _role;
        private readonly IPermissionService _permission;
        private readonly IHttpContextAccessor _httpCont;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly JwtModel _jwt;
        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;

        public AuthService(IGuidRepository<Customer> baseRepo, ICustomerRepository repo, ICustomerService customer,
            IRoleService role, IPermissionService permission, IHttpContextAccessor httpCont, IPasswordHasher passwordHasher, IMapper mapper, IOptions<JwtModel> jwt)
        {
            _baseRepo = baseRepo;
            _customerRepo = repo;
            _customer = customer;
            _role = role;
            _permission = permission;
            _httpCont = httpCont;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _jwt = jwt.Value;
        }

        public async Task<CustomerViewModel> Login(LoginViewModal request)
        {
            var customer = await _customerRepo.GetByUsername(request.Username);
            if (customer == null)
            {
                return null;
            }

            var passwordHash = _passwordHasher.ComputeHash(request.Password, customer.password_salt, _papper, _iteration);
            if (passwordHash != customer.password_hash)
            {
                return null;
            }
            return _mapper.Map<CustomerViewModel>(customer);
        }
        public async Task<string> GenerateAccessToken(string username, Guid customerId, int roleId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, roleId.ToString()),
            };

            var tokenDescriptor = new JwtSecurityToken
                (
                    _jwt.Issuer,
                    _jwt.Audience,
                    claims: claims,
                    expires : DateTime.Now.AddMinutes(_jwt.ExpiryMinutes),
                    signingCredentials: credential
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenDescriptor);
        }
        public async Task<CustomerViewModel> Register(CustomerViewModel request)
        {
            var role = await _role.GetById(request.RoleId);
            var customer = new Customer
            {
                role_id = role.Id,
                role_name = role.Name,
                username = request.Username,
                email = request.Email,
                password_salt = _passwordHasher.GenerateSalt(),
                full_name = request.FullName,
                phone_number = request.PhoneNumber,
                address = request.Address,
                created_by = request.CreatedBy,
                modified_by = request.ModifiedBy,
            };

            customer.password_hash = _passwordHasher.ComputeHash(request.Password, customer.password_salt, _papper, _iteration);
            _baseRepo.Create(customer);
            var res = _mapper.Map<CustomerViewModel>(customer);
            res.Password = "==HASH==";
            return res;
        }
        public async Task<bool> IsRequestPermitted()
        {
            var user = _httpCont?.HttpContext?.User;
            var username = user?.FindFirst(ClaimTypes.Name)?.Value;
            var path = _httpCont.HttpContext.Request.Path.Value;
            var method = _httpCont.HttpContext.Request.Method.ToString();

            var splitPath = path.Split('/');

            var allUser = await _customer.GetAll();
            var allPermission = await _permission.GetAll();
            var allRole = await _role.GetAll();

            var usr = allUser.FirstOrDefault(x => x.Username == username);
            if(usr == null) {
                return true;
            }
            if(usr.RoleId == 1)
            {
                return true;
            }

            bool result;
            if (splitPath.Length < 2)
            {
                result = true;
            }
            else
            {
                var controller = splitPath[0];
                var action = splitPath[1];

            }

            return true;
        }
    }
}
