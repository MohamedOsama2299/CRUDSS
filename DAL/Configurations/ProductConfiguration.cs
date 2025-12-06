using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(10,5)");

            builder.Property(p => p.Tax)
                   .HasColumnType("decimal(10,5)");

            builder.Property(p => p.Advertisement)
                   .HasColumnType("decimal(10,5)");

            builder.Property(p => p.Discount)
                   .HasColumnType("decimal(10,5)");

            builder.Property(p => p.StockQuantity)
                   .IsRequired();

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
