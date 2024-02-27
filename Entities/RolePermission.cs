using Marketplace.Enitities.Base;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Enitities
{
    public class RolePermission
    {
        public int id { get; set; }
        public int role_id { get; set; }
        public string permission { get; set; }
    }
}
