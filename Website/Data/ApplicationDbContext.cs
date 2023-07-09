using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<FoodItem> FoodItems { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<CartItem> Cart { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "David",
                    Email = "david@gmail.com",
                    Password = "david123",
                });
        }
    }
}
