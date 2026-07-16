namespace ECommerceAPI.Application.DTOs.Order;

public class CreateOrderDto
{
    public string BuyerEmail { get; set; } = string.Empty;
    public string BasketId { get; set; } = string.Empty;
    public int DeliveryMethodId { get; set; }
    public AddressDto ShippingAddress { get; set; } = null!;
}
