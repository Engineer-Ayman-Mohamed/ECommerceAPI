using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Brands.Commands;

public record DeleteBrandCommand(int Id) : IRequest<Result<bool>>;
public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBrandCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(request.Id);

        if (brand is null)
            return Result<bool>.Fail($"Brand with ID {request.Id} not found", ErrorCodes.BrandNotFound);

        if (brand.Products.Any())
            return Result<bool>.Fail("Cannot delete brand with associated products", ErrorCodes.BrandHasProducts);

        _unitOfWork.Repository<ProductBrand>().Delete(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Ok(true);
    }
}
