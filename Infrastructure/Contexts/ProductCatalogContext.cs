using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class ProductCatalogContext : DbContext
{
    public ProductCatalogContext()
    {
    }

    public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ProductsEntity> ProductsEntities { get; set; }

    public virtual DbSet<StoresEntity> StoresEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Anna\\Documents\\Repos\\EFCApplication\\Infrastructure\\Data\\ProductCatalog.mdf;Integrated Security=True;Connect Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductsEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC077597133C");

            entity.ToTable("ProductsEntity");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductName).HasMaxLength(50);

            entity.HasOne(d => d.Store).WithMany(p => p.ProductsEntities)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__ProductsE__Store__45F365D3");
        });

        modelBuilder.Entity<StoresEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StoresEn__3214EC07C0C195B0");

            entity.ToTable("StoresEntity");

            entity.HasIndex(e => e.StoreName, "UQ__StoresEn__520DB652A319ED01").IsUnique();

            entity.Property(e => e.StoreName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
