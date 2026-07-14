using AutoMapper;
using ECommerceAPI.Application.DTOs.DeliveryMethod;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Mappings;

public class DeliveryMethodMappingProfile : Profile
{
    public DeliveryMethodMappingProfile()
    {
        CreateMap<DeliveryMethod, DeliveryMethodToReturnDto>();
    }
}
