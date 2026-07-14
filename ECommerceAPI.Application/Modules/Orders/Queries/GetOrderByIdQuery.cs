using AutoMapper;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities.OrderAggregate;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Orders.Queries;

public record GetOrderByIdQuery(int Id) : IRequest<Result<OrderToReturnDto>>;
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderToReturnDto>>
{
    private readonly IGenericRepository<Order> _orderRepo;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IGenericRepository<Order> orderRepo, IMapper mapper)
    {
        _orderRepo = orderRepo;
        _mapper = mapper;
    }

    public async Task<Result<OrderToReturnDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.GetByIdAsync(request.Id);

        if (order is null)
            return Result<OrderToReturnDto>.Fail($"Order with ID {request.Id} not found", ErrorCodes.OrderNotFound);

        return Result<OrderToReturnDto>.Ok(_mapper.Map<OrderToReturnDto>(order));
    }
}
