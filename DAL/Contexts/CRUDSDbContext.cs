using DAL.Configurations;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contexts
{
  public class CRUDSDbContext : DbContext
    {
        public CRUDSDbContext(DbContextOptions<CRUDSDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
