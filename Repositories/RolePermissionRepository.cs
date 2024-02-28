using Marketplace.Enitities;
using Marketplace.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Marketplace.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly DataContext _context;
        public RolePermissionRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task<List<RolePermission>> GetAll()
        {
            return await _context.RolePermissions.ToListAsync();
        }


    }
}
