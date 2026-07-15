using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Infrastructure.Data.Config;

public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
