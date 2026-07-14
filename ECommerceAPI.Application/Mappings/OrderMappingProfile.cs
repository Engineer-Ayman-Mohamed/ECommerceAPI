using AutoMapper;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Domain.Entities.OrderAggregate;

namespace ECommerceAPI.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderToReturnDto>()
            .ForMember(d => d.DeliveryMethod, opt => opt.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

        CreateMap<OrderItem, OrderItemToReturnDto>();

        CreateMap<AddressDto, Address>()
            .ConstructUsing(src => new Address
            {
                FirstName = src.FirstName,
                LastName = src.LastName,
                Street = src.Street,
                City = src.City,
                State = src.State,
                ZipCode = src.ZipCode
            });
    }
}
