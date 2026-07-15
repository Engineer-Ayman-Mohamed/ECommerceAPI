using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Infrastructure.Data.Config;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(d => d.ShortName)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.DeliveryTime)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Price)
            .HasColumnType("decimal(18,2)");
    }
}
