using AutoMapper;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Domain.Specifications;
using MediatR;

namespace ECommerceAPI.Application.Modules.Products.Queries;

public record GetProductsQuery(int? BrandId, int? TypeId, string? Sort, int PageIndex = 1, int PageSize = 10)
    : IRequest<Result<Pagination<ProductToReturnDto>>>;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, Result<Pagination<ProductToReturnDto>>>
{
    private readonly IGenericRepository<Domain.Entities.Product> _productRepo;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IGenericRepository<Domain.Entities.Product> productRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<Result<Pagination<ProductToReturnDto>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken
    ) {
        var spec = new ProductsWithFilters(request.BrandId, request.TypeId, request.Sort, request.PageIndex, request.PageSize);
        var countSpec = new ProductsWithFiltersCount(request.BrandId, request.TypeId);

        var totalItems = await _productRepo.CountAsync(countSpec);
        var products = await _productRepo.ListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

        return Result<Pagination<ProductToReturnDto>>.Ok(new Pagination<ProductToReturnDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            Count = totalItems,
            Data = data
        });
    }
}
