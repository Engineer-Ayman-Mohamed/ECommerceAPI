using AutoMapper;
using ECommerceAPI.Application.DTOs.Basket;
using ECommerceAPI.Domain.Entities.Basket;

namespace ECommerceAPI.Application.Mappings;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<CustomerBasket, CustomerBasketDto>();
        CreateMap<ProductItem, BasketItemDto>();
        CreateMap<BasketItemDto, ProductItem>();
        CreateMap<CustomerBasketDto, CustomerBasket>();
    }
}
