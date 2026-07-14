using AutoMapper;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.TypeName, opt => opt.MapFrom(s => s.ProductType.Name));

        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}
