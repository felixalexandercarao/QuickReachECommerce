using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using System;
using Microsoft.EntityFrameworkCore.SqlServer;
using QuickReach.ECommerce.Infra.Data.EntityConfiguration;
using System.Linq;

namespace QuickReach.ECommerce.Infra.Data
{
    public class ECommerceDbContext : DbContext
    {
       public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        public ECommerceDbContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString =
            "Server=.;Database=QuickReachDb;Integrated Security=true;";

                optionsBuilder.UseSqlServer(connectionString);

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryRollupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductSupplierEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfigration());
            modelBuilder.ApplyConfiguration(new ManufacturerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductManufacturerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemEntityTypeConfiguration());
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }    
        }
        

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
