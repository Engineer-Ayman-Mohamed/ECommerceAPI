using Ardalis.Specification;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Domain.Specifications;

public class ProductsWithBrandAndType : Specification<Product>
{
    public ProductsWithBrandAndType()
    {
        Query.Include(p => p.ProductBrand)
             .Include(p => p.ProductType)
             .OrderBy(p => p.Name);
    }
}
