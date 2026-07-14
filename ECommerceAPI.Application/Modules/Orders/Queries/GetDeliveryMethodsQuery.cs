using AutoMapper;
using ECommerceAPI.Application.DTOs.DeliveryMethod;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Orders.Queries;

public record GetDeliveryMethodsQuery : IRequest<Result<List<DeliveryMethodToReturnDto>>>;
public class GetDeliveryMethodsQueryHandler : IRequestHandler<GetDeliveryMethodsQuery, Result<List<DeliveryMethodToReturnDto>>>
{
    private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
    private readonly IMapper _mapper;

    public GetDeliveryMethodsQueryHandler(IGenericRepository<DeliveryMethod> deliveryMethodRepo, IMapper mapper)
    {
        _deliveryMethodRepo = deliveryMethodRepo;
        _mapper = mapper;
    }

    public async Task<Result<List<DeliveryMethodToReturnDto>>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
    {
        var methods = await _deliveryMethodRepo.GetAllAsync();
        return Result<List<DeliveryMethodToReturnDto>>.Ok(_mapper.Map<List<DeliveryMethodToReturnDto>>(methods));
    }
}
