using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Hubtel.eCommerce.Carts.Api.Models;
using Microsoft.Extensions.Configuration;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hubtel.eCommerce.Carts.Api.Data
{
    public partial class eCommerceContext : DbContext
    {
        private readonly IConfiguration _config;
        public eCommerceContext()
        {
        }

        public eCommerceContext(DbContextOptions<eCommerceContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<Cart> Cart { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_config.GetConnectionString("eCommerceDB"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PRIMARY");

                entity.ToTable("cart");

                entity.Property(e => e.ItemId)
                    .HasColumnName("ITEM_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ItemName)
                    .HasColumnName("ITEM_NAME")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Quantity)
                    .HasColumnName("QUANTITY")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("UNIT_PRICE")
                    .HasColumnType("decimal(10,2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
