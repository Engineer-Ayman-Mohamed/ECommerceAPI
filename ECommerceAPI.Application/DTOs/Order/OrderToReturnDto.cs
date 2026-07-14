using ECommerceAPI.Domain.Enums;

namespace ECommerceAPI.Application.DTOs.Order;

public class OrderToReturnDto
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public AddressDto ShippingAddress { get; set; } = null!;
    public string DeliveryMethod { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<OrderItemToReturnDto> Items { get; set; } = new();
}
