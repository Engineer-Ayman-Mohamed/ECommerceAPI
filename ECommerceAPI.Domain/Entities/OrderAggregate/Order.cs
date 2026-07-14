using ECommerceAPI.Domain.Enums;

namespace ECommerceAPI.Domain.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public string BuyerEmail { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public Address ShippingAddress { get; set; } = null!;
    public int DeliveryMethodId { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; } = null!;
    public decimal Subtotal { get; set; }
    public decimal Total => Subtotal + DeliveryMethod.Price;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string PaymentIntentId { get; set; } = string.Empty;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
