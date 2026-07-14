using AutoMapper;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Entities.OrderAggregate;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Orders.Queries;

public record GetOrdersForUserQuery(string BuyerEmail) : IRequest<Result<List<OrderToReturnDto>>>;
public class GetOrdersForUserQueryHandler : IRequestHandler<GetOrdersForUserQuery, Result<List<OrderToReturnDto>>>
{
    private readonly IGenericRepository<Order> _orderRepo;
    private readonly IMapper _mapper;

    public GetOrdersForUserQueryHandler(IGenericRepository<Order> orderRepo, IMapper mapper)
    {
        _orderRepo = orderRepo;
        _mapper = mapper;
    }

    public async Task<Result<List<OrderToReturnDto>>> Handle(GetOrdersForUserQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepo.GetAllAsync();
        var userOrders = orders.Where(o => o.BuyerEmail == request.BuyerEmail).ToList();
        return Result<List<OrderToReturnDto>>.Ok(_mapper.Map<List<OrderToReturnDto>>(userOrders));
    }
}
