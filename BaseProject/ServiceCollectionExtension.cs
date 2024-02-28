using Bogus;
using Marketplace.Repositories;
using Marketplace.Repositories.Base;
using Marketplace.Requests;
using Marketplace.Responses;
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

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<Faker<ProductViewModel>, Faker<ProductViewModel>>();
            services.AddScoped(typeof(IDummyGenerator<>), typeof(DummyGenerator<>));

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<DataContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpContextAccessor();
        }
    }
}
