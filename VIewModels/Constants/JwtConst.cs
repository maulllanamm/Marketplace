using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ants
{
    public class JwtModel
    {
        public  string Secret { get; set; }
        public  string Issuer { get; set; }
        public  string Audience { get; set; }
        public  int ExpiryMinutes { get; set; }
        public  int RefreshExpiration { get; set; }
        public  string CustomerId { get; set; }
        public  string RoleId { get; set; }
    }
}
