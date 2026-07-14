using Ardalis.Specification;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Domain.Specifications;

public class ProductsWithFilters : Specification<Product>
{
    public ProductsWithFilters(int? brandId, int? typeId, string? sort)
    {
        Query.Include(p => p.ProductBrand)
             .Include(p => p.ProductType);

        if (brandId.HasValue)
            Query.Where(p => p.ProductBrandId == brandId.Value);

        if (typeId.HasValue)
            Query.Where(p => p.ProductTypeId == typeId.Value);

        Query.OrderBy(p => p.Name);

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort.ToLower())
            {
                case "priceasc":
                    Query.OrderBy(p => p.Price);
                    break;
                case "pricedesc":
                    Query.OrderByDescending(p => p.Price);
                    break;
            }
        }
    }
}
