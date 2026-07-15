using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Infrastructure.Data.Config;

public class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
{
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
