using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ProductName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.ModifiedBy)
            .HasMaxLength(100);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);

        builder.HasIndex(x => x.ProductName)
    .HasDatabaseName("IX_Product_ProductName");

        builder.HasIndex(x => x.CreatedOn)
            .HasDatabaseName("IX_Product_CreatedOn");
    }
}