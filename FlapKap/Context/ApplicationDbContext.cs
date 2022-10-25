using FlapKap.Models;
using Microsoft.EntityFrameworkCore;

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
}
