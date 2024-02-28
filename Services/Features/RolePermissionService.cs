using AutoMapper;
using Marketplace.Enitities;
using Marketplace.Responses;
using Marketplace.Services.Base;
using Marketplace.Services.Interface;
using Repositories.Base;
using Repositories.Interface;

namespace Marketplace.Requests
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _repo;
        private readonly IMapper _mapper;
        public RolePermissionService(IMapper mapper, IRolePermissionRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        public async Task<List<RolePermission>> GetAll()
        {
            return await _repo.GetAll();
        }
    }
}
