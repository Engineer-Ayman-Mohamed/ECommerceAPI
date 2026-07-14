using AutoMapper;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Domain.Specifications;
using MediatR;

namespace ECommerceAPI.Application.Modules.Products.Queries;

public record GetProductsQuery(int? BrandId, int? TypeId, string? Sort) : IRequest<Result<List<ProductToReturnDto>>>;
public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ProductToReturnDto>>>
{
    private readonly IGenericRepository<Domain.Entities.Product> _productRepo;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IGenericRepository<Domain.Entities.Product> productRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductToReturnDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductsWithFilters(request.BrandId, request.TypeId, request.Sort);
        var products = await _productRepo.ListAsync(spec);
        return Result<List<ProductToReturnDto>>.Ok(_mapper.Map<List<ProductToReturnDto>>(products));
    }
}
