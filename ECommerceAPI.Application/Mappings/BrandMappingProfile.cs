using AutoMapper;
using ECommerceAPI.Application.DTOs.Brand;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Mappings;

public class BrandMappingProfile : Profile
{
    public BrandMappingProfile()
    {
        CreateMap<ProductBrand, BrandToReturnDto>();
        CreateMap<CreateBrandDto, ProductBrand>();
    }
}
