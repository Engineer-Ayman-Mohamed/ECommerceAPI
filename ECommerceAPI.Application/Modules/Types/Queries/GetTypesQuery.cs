using AutoMapper;
using ECommerceAPI.Application.DTOs.Type;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Types.Queries;

public record GetTypesQuery : IRequest<Result<List<TypeToReturnDto>>>;
public class GetTypesQueryHandler : IRequestHandler<GetTypesQuery, Result<List<TypeToReturnDto>>>
{
    private readonly IGenericRepository<ProductType> _typeRepo;
    private readonly IMapper _mapper;

    public GetTypesQueryHandler(IGenericRepository<ProductType> typeRepo, IMapper mapper)
    {
        _typeRepo = typeRepo;
        _mapper = mapper;
    }

    public async Task<Result<List<TypeToReturnDto>>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _typeRepo.GetAllAsync();
        return Result<List<TypeToReturnDto>>.Ok(_mapper.Map<List<TypeToReturnDto>>(types));
    }
}
