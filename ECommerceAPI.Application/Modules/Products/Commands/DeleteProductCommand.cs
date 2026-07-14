using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Products.Commands;

public record DeleteProductCommand(int Id) : IRequest<Result<bool>>;
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id);

        if (product is null)
            return Result<bool>.Fail($"Product with ID {request.Id} not found", ErrorCodes.ProductNotFound);

        _unitOfWork.Repository<Product>().Delete(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Ok(true);
    }
}
