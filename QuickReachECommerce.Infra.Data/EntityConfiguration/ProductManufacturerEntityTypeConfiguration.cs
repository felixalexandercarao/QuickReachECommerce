using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
    public class ProductManufacturerEntityTypeConfiguration : IEntityTypeConfiguration<ProductManufacturer>
    {
        public void Configure(EntityTypeBuilder<ProductManufacturer> builder)
        {
            builder.ToTable("ProductManufacturer");

            builder.HasKey(cr => new { cr.ManufacturerID, cr.ProductID });

            builder.HasOne(cr => cr.Manufacturer)
                   .WithMany(c => c.ProductManufacturers)
                   .HasForeignKey("ManufacturerID");

            builder.HasOne(cr => cr.Product)
                   .WithMany(c => c.ProductManufacturers)
                   .HasForeignKey("ProductID");
        }
    }
}
