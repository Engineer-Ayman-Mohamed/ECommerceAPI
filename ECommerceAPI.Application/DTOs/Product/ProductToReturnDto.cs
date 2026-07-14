namespace ECommerceAPI.Application.DTOs.Product;

public class ProductToReturnDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
}
