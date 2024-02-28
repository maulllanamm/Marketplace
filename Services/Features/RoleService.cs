using AutoMapper;
using Bogus;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;
using Repositories.Interface;
using System.Diagnostics;

namespace Marketplace.Requests
{
    public class RoleService : Service<RoleViewModel, Role>, IRoleService
    {
        private readonly IRepository<Role> _baseRepo;
        private readonly IMapper _mapper;
        public RoleService(IRepository<Role> baseRepo, IMapper mapper) : base(mapper, baseRepo)
        {
            _baseRepo = baseRepo;
            _mapper = mapper;
        }

    }
}
