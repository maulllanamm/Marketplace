using Marketplace.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfigurasi nama tabel
            modelBuilder.Entity<Customer>().ToTable("customer");

            base.OnModelCreating(modelBuilder);

        }


    }


}
