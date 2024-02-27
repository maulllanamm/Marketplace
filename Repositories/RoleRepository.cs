using Marketplace.Enitities;
using Marketplace.Repositories.Base;
using Repositories.Interface;

namespace Marketplace.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly DataContext _context;
        public RoleRepository(DataContext context) : base(context)
        {
            _context = context;
        }

       
    }
}
