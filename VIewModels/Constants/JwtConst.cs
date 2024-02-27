using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Constants
{
    public class JwtConst
    {
        public const string Secret = "65nrx97WPnLiotrT1MSdSDjUOPmR3XtkgsuPRtqc2bmVkS6mCnzq8AMf2hySraxN";
        public const string Issuer = "https://juldhais.net";
        public const string Audience = "https://juldhais.net";
        public const int ExpiryMinutes = 60;
        public const string CustomerId = "cid";
        public const string RoleId = "rid";
    }
}
