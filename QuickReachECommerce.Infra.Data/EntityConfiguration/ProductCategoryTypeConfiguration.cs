using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
    public class ProductCategoryEntityTypeConfiguration
        : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategory");

            builder.HasKey(cr => new { cr.CategoryID, cr.ProductID });
            builder.HasOne(cr => cr.Category)
                   .WithMany(c => c.ProductCategories)
                   .HasForeignKey("CategoryID");

            builder.HasOne(cr => cr.Product)
                   .WithMany(c => c.ProductCategories)
                   .HasForeignKey("ProductID");
        }
    }
}
