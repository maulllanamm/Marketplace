using Marketplace.Repositories;
using Marketplace.Repositories.Base;
using Marketplace.Requests;
using Marketplace.Services.Interface;
using Repositories.Base;
using Repositories.ConfigUoW;
using Repositories.Interface;

namespace Marketplace
{
    public static class ServiceCollectionExtension
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGuidRepository<>), typeof(GuidRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<DataContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
