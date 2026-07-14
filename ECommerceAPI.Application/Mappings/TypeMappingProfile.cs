using AutoMapper;
using ECommerceAPI.Application.DTOs.Type;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Mappings;

public class TypeMappingProfile : Profile
{
    public TypeMappingProfile()
    {
        CreateMap<ProductType, TypeToReturnDto>();
        CreateMap<CreateTypeDto, ProductType>();
    }
}
