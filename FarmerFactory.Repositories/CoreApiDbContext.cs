using FarmerFactory.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerFactory.Repositories;

public class CoreApiDbContext : DbContext
{
    public const string DataProtectionKeySchemaName = "is6";

    public CoreApiDbContext(DbContextOptions<CoreApiDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductType> ProductTypes { get; set; }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Purchase> Purchases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>();

        modelBuilder.Entity<Client>();

        modelBuilder.Entity<Purchase>();

        modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Grade A", Description = "Good Quality Corn", Price = 12, Stock = 1150, ProductTypeId = 1 }
                );

        modelBuilder.Entity<ProductType>().HasData(
               new ProductType { Id = 1, Name = "Grains" }
               );

        modelBuilder.Entity<Client>().HasData(
              new Client { Id = 1, Name = "Client A" },
              new Client { Id = 2, Name = "Client B" },
              new Client { Id = 3, Name = "Client C" }
              );
    }
}