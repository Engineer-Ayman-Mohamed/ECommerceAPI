namespace ECommerceAPI.Application.DTOs.Order;

public class OrderItemToReturnDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
