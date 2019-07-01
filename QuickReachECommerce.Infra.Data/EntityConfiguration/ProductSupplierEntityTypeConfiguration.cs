using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
    public class ProductSupplierEntityTypeConfiguration
        : IEntityTypeConfiguration<ProductSupplier>
    {
        public void Configure(EntityTypeBuilder<ProductSupplier> builder)
        {
            builder.ToTable("ProductSupplier");

            builder.HasKey(cr => new { cr.SupplierID, cr.ProductID });

            builder.HasOne(cr => cr.Supplier)
                   .WithMany(c => c.ProductSuppliers)
                   .HasForeignKey("SupplierID");

            builder.HasOne(cr => cr.Product)
                   .WithMany(c => c.ProductSuppliers)
                   .HasForeignKey("ProductID");
        }
    }
}
