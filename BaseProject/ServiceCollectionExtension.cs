using Marketplace.Repositories.Base;
using Marketplace.Requests;
using Marketplace.Services.Interface;
using Repositories.Base;

namespace Marketplace
{
    public static class ServiceCollectionExtension
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseGuidRepository<>), typeof(BaseGuidRepository<>));
            services.AddScoped(typeof(IBaseIdRepository<>), typeof(BaseIdRepository<>));
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}
