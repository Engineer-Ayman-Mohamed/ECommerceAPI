using AutoMapper;
using ECommerceAPI.Application.DTOs.Brand;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Brands.Queries;

public record GetBrandsQuery : IRequest<Result<List<BrandToReturnDto>>>;
public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, Result<List<BrandToReturnDto>>>
{
    private readonly IGenericRepository<ProductBrand> _brandRepo;
    private readonly IMapper _mapper;

    public GetBrandsQueryHandler(IGenericRepository<ProductBrand> brandRepo, IMapper mapper)
    {
        _brandRepo = brandRepo;
        _mapper = mapper;
    }

    public async Task<Result<List<BrandToReturnDto>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _brandRepo.GetAllAsync();
        return Result<List<BrandToReturnDto>>.Ok(_mapper.Map<List<BrandToReturnDto>>(brands));
    }
}
