using AutoMapper;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.OrderAggregate;
using ECommerceAPI.Domain.Enums;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Orders.Commands;

public record CreateOrderCommand(CreateOrderDto Order) : IRequest<Result<OrderToReturnDto>>;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderToReturnDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketRepository _basketRepo;
    private readonly IMapper _mapper;
    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IBasketRepository basketRepo, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _basketRepo = basketRepo;
        _mapper = mapper;
    }

    public async Task<Result<OrderToReturnDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepo.GetBasketAsync(request.Order.BasketId);

        if (basket is null)
            return Result<OrderToReturnDto>.Fail("Basket not found", ErrorCodes.BasketNotFound);

        if (!basket.Items.Any())
            return Result<OrderToReturnDto>.Fail("Basket is empty", ErrorCodes.EmptyBasket);

        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(request.Order.DeliveryMethodId);

        if (deliveryMethod is null)
            return Result<OrderToReturnDto>.Fail("Delivery method not found", ErrorCodes.DeliveryMethodNotFound);

        var address = _mapper.Map<Address>(request.Order.ShippingAddress);
        var subtotal = basket.Items.Sum(item => item.Price * item.Quantity);

        var order = new Order
        {
            BuyerEmail = "user@example.com",
            ShippingAddress = address,
            DeliveryMethodId = deliveryMethod.Id,
            Subtotal = subtotal,
            Status = OrderStatus.Pending,
            Items = basket.Items.Select(item => new OrderItem
            {
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl,
                UnitPrice = item.Price,
                Quantity = item.Quantity
            }).ToList()
        };

        await _unitOfWork.Repository<Order>().AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<OrderToReturnDto>.Ok(_mapper.Map<OrderToReturnDto>(order));
    }
}
