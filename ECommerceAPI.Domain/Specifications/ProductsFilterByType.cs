using Ardalis.Specification;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Domain.Specifications;

public class ProductsFilterByType : Specification<Product>
{
    public ProductsFilterByType(int? typeId)
    {
        Query.Include(p => p.ProductBrand)
             .Include(p => p.ProductType);

        if (typeId.HasValue)
            Query.Where(p => p.ProductTypeId == typeId.Value);

        Query.OrderBy(p => p.Name);
    }
}
