using Marketplace.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfigurasi nama tabel
            modelBuilder.Entity<Customer>().ToTable("customer");
            modelBuilder.Entity<Product>().ToTable("product");
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Permission>().ToTable("permissions");

            base.OnModelCreating(modelBuilder);

        }


    }


}
