using Marketplace.Enitities;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IRolePermissionRepository 
    {
        public Task<List<RolePermission>> GetAll();
    }
}
