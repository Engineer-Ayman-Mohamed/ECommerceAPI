namespace ECommerceAPI.Application.DTOs.DeliveryMethod;

public class DeliveryMethodToReturnDto
{
    public int Id { get; set; }
    public string ShortName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeliveryTime { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
