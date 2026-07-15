using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerceAPI.Domain.Entities.OrderAggregate;

namespace ECommerceAPI.Infrastructure.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.ShippingAddress, a =>
        {
            a.Property(s => s.FirstName).IsRequired().HasMaxLength(50);
            a.Property(s => s.LastName).IsRequired().HasMaxLength(50);
            a.Property(s => s.Street).IsRequired().HasMaxLength(100);
            a.Property(s => s.City).IsRequired().HasMaxLength(50);
            a.Property(s => s.State).IsRequired().HasMaxLength(50);
            a.Property(s => s.ZipCode).IsRequired().HasMaxLength(20);
        });

        builder.Property(o => o.BuyerEmail)
            .IsRequired();

        builder.Property(o => o.Subtotal)
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.PaymentIntentId)
            .IsRequired();

        builder.HasOne(o => o.DeliveryMethod)
            .WithMany()
            .HasForeignKey(o => o.DeliveryMethodId);

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId);
    }
}
