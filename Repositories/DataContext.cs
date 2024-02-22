using Marketplace.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<CustomerEntity> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfigurasi nama tabel
            modelBuilder.Entity<CustomerEntity>().ToTable("customer");

            base.OnModelCreating(modelBuilder);

        }


    }


}
