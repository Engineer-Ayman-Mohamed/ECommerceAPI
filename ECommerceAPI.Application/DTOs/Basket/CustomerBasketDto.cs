namespace ECommerceAPI.Application.DTOs.Basket;

public class CustomerBasketDto
{
    public string Id { get; set; } = string.Empty;
    public List<BasketItemDto> Items { get; set; } = new();
    public int? DeliveryMethodId { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
}
