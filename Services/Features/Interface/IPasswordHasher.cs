using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Services.Base;

namespace Marketplace.Services.Interface
{
    public interface IPasswordHasher 
    {
        public string ComputeHash(string password, string salt, string pepper, int iteration);
        public string GenerateSalt();
    }
}
