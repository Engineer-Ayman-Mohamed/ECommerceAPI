using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Basket.Commands;

public record DeleteBasketCommand(string BasketId) : IRequest<Result<bool>>;
public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Result<bool>>
{
    private readonly IBasketRepository _basketRepo;

    public DeleteBasketCommandHandler(IBasketRepository basketRepo)
    {
        _basketRepo = basketRepo;
    }

    public async Task<Result<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _basketRepo.DeleteBasketAsync(request.BasketId);
            return Result<bool>.Ok(true);
        }
        catch (Exception)
        {
            return Result<bool>.Fail("Failed to delete basket", ErrorCodes.BasketDeleteFailed);
        }
    }
}
