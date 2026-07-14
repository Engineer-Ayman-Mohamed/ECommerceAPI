using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Types.Commands;

public record DeleteTypeCommand(int Id) : IRequest<Result<bool>>;
public class DeleteTypeCommandHandler : IRequestHandler<DeleteTypeCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTypeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteTypeCommand request, CancellationToken cancellationToken)
    {
        var type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(request.Id);

        if (type is null)
            return Result<bool>.Fail($"Type with ID {request.Id} not found", ErrorCodes.TypeNotFound);

        if (type.Products.Any())
            return Result<bool>.Fail("Cannot delete type with associated products", ErrorCodes.TypeHasProducts);

        _unitOfWork.Repository<ProductType>().Delete(type);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Ok(true);
    }
}
