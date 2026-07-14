namespace ECommerceAPI.Application.DTOs.Order;

public class CreateOrderDto
{
    public string BasketId { get; set; } = string.Empty;
    public int DeliveryMethodId { get; set; }
    public AddressDto ShippingAddress { get; set; } = null!;
}
