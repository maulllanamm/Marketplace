using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;
using Repositories.Interface;

namespace Marketplace.Requests
{
    public class CustomerService : GuidService<CustomerViewModel, Customer>, ICustomerService
    {
        private readonly IGuidRepository<Customer> _baseRepo ;
        private readonly ICustomerRepository _repo ;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;
        public CustomerService(IMapper mapper, IPasswordHasher passwordHasher, IGuidRepository<Customer> baseRepo, ICustomerRepository repo) : base(mapper, baseRepo)
        {
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _baseRepo = baseRepo;
            _repo = repo;
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await _repo.GetByUsername(username);
            if (user == null)
            {
                return "Username or password did not match.";
            }

            var passwordHash = _passwordHasher.ComputeHash(password, user.password_salt, _papper, _iteration);
            if(passwordHash != user.password_hash)
            {
                return "Username or password did not match.";
            }
            return "Login Success!";
        }

        public async Task<CustomerViewModel> Register(CustomerViewModel request)
        {
            var customer = new Customer
            {
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
        public async Task<CustomerViewModel> Edit(CustomerViewModel request)
        {
            var customer = _mapper.Map<Customer>(request);
            _baseRepo.Edit(customer);
            return request;
        }

        public async Task<Guid> Delete(Guid id)
        {
            _baseRepo.Delete(id);
            return id;
        }
    }
}
