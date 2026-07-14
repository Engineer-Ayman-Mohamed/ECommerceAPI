namespace ECommerceAPI.Domain.Entities.Basket;

public class CustomerBasket : BaseEntity
{
    public new string Id { get; set; } = Guid.NewGuid().ToString();
    public ICollection<ProductItem> Items { get; set; } = new List<ProductItem>();
    public int? DeliveryMethodId { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
}
