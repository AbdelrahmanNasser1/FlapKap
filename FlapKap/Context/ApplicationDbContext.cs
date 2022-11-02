using FlapKap.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlapKap.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(e =>
            {
                e.HasData(new[]
                {
                new Role() { Id = 1, Name="Seller" },
                new Role() { Id = 2, Name="Buyer" },
            });
            });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=ABDELRAHMAN-NAS\\MSSQLSERVER15;Initial Catalog=FlapKap;Integrated Security=True;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
