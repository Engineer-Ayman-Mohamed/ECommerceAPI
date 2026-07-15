using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Infrastructure.Data.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PictureUrl)
            .IsRequired();

        builder.HasOne(p => p.ProductBrand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.ProductBrandId);

        builder.HasOne(p => p.ProductType)
            .WithMany(t => t.Products)
            .HasForeignKey(p => p.ProductTypeId);
    }
}
