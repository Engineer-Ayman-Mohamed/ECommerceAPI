using AutoMapper;
using ECommerceAPI.Application.DTOs.Basket;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Entities.Basket;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Basket.Commands;

public record UpdateBasketCommand(CustomerBasketDto Basket) : IRequest<Result<CustomerBasketDto>>;
public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, Result<CustomerBasketDto>>
{
    private readonly IBasketRepository _basketRepo;
    private readonly IMapper _mapper;

    public UpdateBasketCommandHandler(IBasketRepository basketRepo, IMapper mapper)
    {
        _basketRepo = basketRepo;
        _mapper = mapper;
    }

    public async Task<Result<CustomerBasketDto>> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = _mapper.Map<CustomerBasket>(request.Basket);
        var updatedBasket = await _basketRepo.UpdateBasketAsync(basket);
        return Result<CustomerBasketDto>.Ok(_mapper.Map<CustomerBasketDto>(updatedBasket));
    }
}
