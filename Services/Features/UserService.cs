using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;
using Repositories.Interface;

namespace Marketplace.Requests
{
    public class UserService : GuidService<UserViewModel, User>, IUserService
    {
        private readonly IGuidRepository<User> _baseRepo ;
        private readonly IUserRepository _repo ;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly string _papper = "v81IKJ3ZBFgwc2AdnYeOLhUn9muUtIQ0";
        private readonly int _iteration = 3;
        public UserService(IMapper mapper, IPasswordHasher passwordHasher, IGuidRepository<User> baseRepo, IUserRepository repo) : base(mapper, baseRepo)
        {
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _baseRepo = baseRepo;
            _repo = repo;
        }

        public async Task<UserViewModel> GetByUsername(string username)
        {
            var entity = await _repo.GetByUsername(username);
            return _mapper.Map<UserViewModel>(entity);
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


        public async Task<UserViewModel> Register(UserViewModel request)
        {
            var user = new User
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

            user.password_hash = _passwordHasher.ComputeHash(request.Password, user.password_salt, _papper, _iteration);
            _baseRepo.Create(user);
            var res = _mapper.Map<UserViewModel>(user);
            res.Password = "==HASH==";
            return res;
        }
        public async Task<UserViewModel> Update(UserViewModel request)
        {
            var user = _mapper.Map<User>(request);
            _baseRepo.Update(user);
            return request;
        }

        public async Task<Guid> Delete(Guid id)
        {
            _baseRepo.Delete(id);
            return id;
        }
    }
}
