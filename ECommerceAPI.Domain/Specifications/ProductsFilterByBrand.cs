using Ardalis.Specification;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Domain.Specifications;

public class ProductsFilterByBrand : Specification<Product>
{
    public ProductsFilterByBrand(int? brandId)
    {
        Query.Include(p => p.ProductBrand)
             .Include(p => p.ProductType);

        if (brandId.HasValue)
            Query.Where(p => p.ProductBrandId == brandId.Value);

        Query.OrderBy(p => p.Name);
    }
}
