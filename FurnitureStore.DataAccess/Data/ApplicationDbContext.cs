using Microsoft.EntityFrameworkCore;
using FurnitureStore.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using FurnitureStore.DataAccess.Migrations;

namespace FurnitureStore.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerUser> Customers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Executive Office Chair",
                    Description = "High-back executive office chair with ergonomic design",
                    Category = "Chairs",
                    ProductCode = "CH001",
                    Supplier = "Supplier A",
                    Manufacturer = "Manufacturer X",
                    ListPrice = 250.00,
                    Price = 250.00,
                    ImageUrl = "",
                    IsAvailableForPurchase = true,
                    Stock = 20
                },
                new Product
                {
                    Id = 2,
                    Name = "Conference Table",
                    Description = "Large rectangular conference table with wooden finish",
                    Category = "Tables",
                    ProductCode = "TB002",
                    Supplier = "Supplier B",
                    Manufacturer = "Manufacturer Y",
                    ListPrice = 800.00,
                    Price = 800.00,
                    ImageUrl = "",
                    IsAvailableForPurchase = true,
                    Stock = 10
                },
                new Product
                {
                    Id = 3,
                    Name = "Bookshelf",
                    Description = "Tall wooden bookshelf with adjustable shelves",
                    Category = "Shelves",
                    ProductCode = "SH003",
                    Supplier = "Supplier C",
                    Manufacturer = "Manufacturer Z",
                    ListPrice = 180.00,
                    Price = 180.00,
                    ImageUrl = "",
                    IsAvailableForPurchase = true,
                    Stock = 50
                },
                new Product
                {
                    Id = 4,
                    Name = "Computer Workstation",
                    Description = "Compact computer workstation with keyboard tray",
                    Category = "Workstations",
                    ProductCode = "WS004",
                    Supplier = "Supplier D",
                    Manufacturer = "Manufacturer W",
                    ListPrice = 300.00,
                    Price = 300.00,
                    ImageUrl = "",
                    IsAvailableForPurchase = true,
                    Stock = 30
                },
                new Product
                {
                    Id = 5,
                    Name = "Mesh Office Chair",
                    Description = "Mesh-back office chair with lumbar support",
                    Category = "Chairs",
                    ProductCode = "CH005",
                    Supplier = "Supplier E",
                    Manufacturer = "Manufacturer Q",
                    ListPrice = 150.00,
                    Price = 150.00,
                    ImageUrl = "",
                    IsAvailableForPurchase = true,
                    Stock = 40
                });

            modelBuilder.Entity<Discount>().HasData(
                new Discount
                {
                    Id = 1,
                    Name = "Standard",
                    Percentage = 0,
                },
                new Discount
                {
                    Id = 2,
                    Name = "Bronze",
                    Percentage = 5,
                },
                new Discount
                {
                    Id = 3,
                    Name = "Silver",
                    Percentage = 10
                },
                new Discount
                {
                    Id = 4,
                    Name = "Gold",
                    Percentage = 15
                });
        }
    }
}
