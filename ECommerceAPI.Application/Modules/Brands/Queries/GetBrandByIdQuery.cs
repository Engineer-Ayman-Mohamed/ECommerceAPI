using AutoMapper;
using ECommerceAPI.Application.DTOs.Brand;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Brands.Queries;

public record GetBrandByIdQuery(int Id) : IRequest<Result<BrandToReturnDto>>;
public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Result<BrandToReturnDto>>
{
    private readonly IGenericRepository<ProductBrand> _brandRepo;
    private readonly IMapper _mapper;

    public GetBrandByIdQueryHandler(IGenericRepository<ProductBrand> brandRepo, IMapper mapper)
    {
        _brandRepo = brandRepo;
        _mapper = mapper;
    }

    public async Task<Result<BrandToReturnDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await _brandRepo.GetByIdAsync(request.Id);

        if (brand is null)
            return Result<BrandToReturnDto>.Fail($"Brand with ID {request.Id} not found", ErrorCodes.BrandNotFound);

        return Result<BrandToReturnDto>.Ok(_mapper.Map<BrandToReturnDto>(brand));
    }
}
