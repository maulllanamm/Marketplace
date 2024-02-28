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
    public class PermissionService : Service<PermissionViewModel, Permission>, IPermissionService
    {
        private readonly IRepository<Permission> _baseRepo;
        private readonly IMapper _mapper;
        public PermissionService(IRepository<Permission> baseRepo, IMapper mapper) : base(mapper, baseRepo)
        {
            _baseRepo = baseRepo;
            _mapper = mapper;
        }

    }
}
