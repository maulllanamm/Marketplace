using Marketplace.Enitities;
using Marketplace.Repositories.Base;
using Repositories.Interface;

namespace Marketplace.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly DataContext _context;
        public PermissionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

       
    }
}
