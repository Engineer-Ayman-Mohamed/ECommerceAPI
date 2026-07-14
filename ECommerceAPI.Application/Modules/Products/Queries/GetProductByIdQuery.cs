using AutoMapper;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Domain.Specifications;
using MediatR;

namespace ECommerceAPI.Application.Modules.Products.Queries;

public record GetProductByIdQuery(int Id) : IRequest<Result<ProductToReturnDto>>;
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductToReturnDto>>
{
    private readonly IGenericRepository<Domain.Entities.Product> _productRepo;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IGenericRepository<Domain.Entities.Product> productRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<Result<ProductToReturnDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductsWithBrandAndType();
        var product = await _productRepo.GetEntityWithSpecAsync(spec);

        if (product is null)
            return Result<ProductToReturnDto>.Fail($"Product with ID {request.Id} not found", ErrorCodes.ProductNotFound);

        return Result<ProductToReturnDto>.Ok(_mapper.Map<ProductToReturnDto>(product));
    }
}
