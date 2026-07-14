using AutoMapper;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities.OrderAggregate;
using ECommerceAPI.Domain.Enums;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Orders.Commands;

public record UpdateOrderStatusCommand(int Id, OrderStatus Status) : IRequest<Result<OrderToReturnDto>>;
public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result<OrderToReturnDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderToReturnDto>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Repository<Order>().GetByIdAsync(request.Id);

        if (order is null)
            return Result<OrderToReturnDto>.Fail($"Order with ID {request.Id} not found", ErrorCodes.OrderNotFound);

        order.Status = request.Status;
        _unitOfWork.Repository<Order>().Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<OrderToReturnDto>.Ok(_mapper.Map<OrderToReturnDto>(order));
    }
}
