using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using Repositories.Base;
using Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViewModels.Constants;

namespace Marketplace.Requests
{
    public class AuthService : IAuthService
    {
        private readonly IGuidRepository<Customer> _baseRepo;
        private readonly ICustomerRepository _repo;
        private readonly IRoleRepository _role;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;

        public AuthService(IGuidRepository<Customer> baseRepo, ICustomerRepository repo, IPasswordHasher passwordHasher, IMapper mapper, IRoleRepository role)
        {
            _baseRepo = baseRepo;
            _repo = repo;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _role = role;
        }

        public async Task<CustomerViewModel> Login(LoginViewModal request)
        {
            var customer = await _repo.GetByUsername(request.Username);
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
        public async Task<string> GenerateAccessToken(Guid customerId, int roleId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConst.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = JwtConst.Issuer,
                Audience = JwtConst.Audience,
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtConst.CustomerId, customerId.ToString()),
                new Claim(JwtConst.RoleId, roleId.ToString())
            }),
                Expires = DateTime.Now.AddMinutes(JwtConst.ExpiryMinutes),
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<CustomerViewModel> Register(CustomerViewModel request)
        {
            var role = await _role.GetById(request.RoleId);
            var customer = new Customer
            {
                role_id = role.id,
                role_name = role.name,
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
    }
}
