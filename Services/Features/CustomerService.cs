using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;

namespace Marketplace.Requests
{
    public class CustomerService : BaseGuidService<CustomerViewModel, CustomerEntity>, ICustomerService
    {
        private readonly IBaseGuidRepository<CustomerEntity> _repo ;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;
        public CustomerService(IMapper mapper, IPasswordHasher passwordHasher, IBaseGuidRepository<CustomerEntity> repo) : base(mapper, repo)
        {
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _repo = repo;
        }

        public async Task<CustomerViewModel> Register(CustomerViewModel request)
        {
            var customer = new CustomerEntity
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
            _repo.Create(customer);
            return _mapper.Map<CustomerViewModel>(customer);
        }
    }
}
