using AutoMapper;
using ECommerceAPI.Application.DTOs.Basket;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Basket.Queries;

public record GetBasketQuery(string BasketId) : IRequest<Result<CustomerBasketDto>>;
public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Result<CustomerBasketDto>>
{
    private readonly IBasketRepository _basketRepo;
    private readonly IMapper _mapper;

    public GetBasketQueryHandler(IBasketRepository basketRepo, IMapper mapper)
    {
        _basketRepo = basketRepo;
        _mapper = mapper;
    }

    public async Task<Result<CustomerBasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepo.GetBasketAsync(request.BasketId);

        if (basket is null)
            return Result<CustomerBasketDto>.Fail($"Basket with ID {request.BasketId} not found", ErrorCodes.BasketNotFound);

        return Result<CustomerBasketDto>.Ok(_mapper.Map<CustomerBasketDto>(basket));
    }
}
