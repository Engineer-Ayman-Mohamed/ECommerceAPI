using AutoMapper;
using ECommerceAPI.Application.DTOs.Type;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Types.Queries;

public record GetTypeByIdQuery(int Id) : IRequest<Result<TypeToReturnDto>>;
public class GetTypeByIdQueryHandler : IRequestHandler<GetTypeByIdQuery, Result<TypeToReturnDto>>
{
    private readonly IGenericRepository<ProductType> _typeRepo;
    private readonly IMapper _mapper;

    public GetTypeByIdQueryHandler(IGenericRepository<ProductType> typeRepo, IMapper mapper)
    {
        _typeRepo = typeRepo;
        _mapper = mapper;
    }

    public async Task<Result<TypeToReturnDto>> Handle(GetTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var type = await _typeRepo.GetByIdAsync(request.Id);

        if (type is null)
            return Result<TypeToReturnDto>.Fail($"Type with ID {request.Id} not found", ErrorCodes.TypeNotFound);

        return Result<TypeToReturnDto>.Ok(_mapper.Map<TypeToReturnDto>(type));
    }
}
