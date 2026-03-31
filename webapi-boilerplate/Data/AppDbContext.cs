using Microsoft.EntityFrameworkCore;
using webapi_boilerplate.Models;

namespace webapi_boilerplate.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Pokemon", Description = "Pokemon figures", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, UpdatedBy = "System" },
            new Category { Id = 2, Name = "Digimon", Description = "Digimon figures", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, UpdatedBy = "System" },
            new Category { Id = 3, Name = "Mobile Suit Gundam", Description = "Mobile Suit Gundam plastic models", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, UpdatedBy = "System" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Pikachu", Price = 49.99M, Stock = 50, CategoryId = 1, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, UpdatedBy = "System" },
            new Product { Id = 2, Name = "Agumon", Price = 29.99M, Stock = 100, CategoryId = 2, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, UpdatedBy = "System" },
            new Product { Id = 3, Name = "Gundam", Price = 199.99M, Stock = 20, CategoryId = 3, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, UpdatedBy = "System" }
        );
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;

}