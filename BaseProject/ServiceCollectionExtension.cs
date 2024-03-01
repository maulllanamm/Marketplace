using Bogus;
using FluentValidation;
using Marketplace.Enitities.Base;
using Marketplace.Repositories;
using Marketplace.Repositories.Base;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Responses.Base;
using Marketplace.Services.Interface;
using Repositories.Base;
using Repositories.ConfigUoW;
using Repositories.Interface;
using WebAPI.Validator;

namespace Marketplace
{
    public static class ServiceCollectionExtension
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGuidRepository<>), typeof(GuidRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<Faker<ProductViewModel>, Faker<ProductViewModel>>();
            services.AddScoped(typeof(IDummyGenerator<>), typeof(DummyGenerator<>));

            services.AddTransient<IValidator<int>, IntegerPayloadValidator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddTransient<DataContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpContextAccessor();
        }
    }
}
