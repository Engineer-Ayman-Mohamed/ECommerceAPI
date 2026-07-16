using AutoMapper;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Domain.Interfaces.Services;
using MediatR;

namespace ECommerceAPI.Application.Modules.Products.Commands;

public record UploadProductImageCommand(int ProductId, Stream FileStream, string FileName)
    : IRequest<Result<ProductToReturnDto>>;

public class UploadProductImageCommandHandler 
    : IRequestHandler<UploadProductImageCommand, Result<ProductToReturnDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPictureService _pictureService;
    private readonly IMapper _mapper;

    public UploadProductImageCommandHandler(
        IUnitOfWork unitOfWork,
        IPictureService pictureService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _pictureService = pictureService;
        _mapper = mapper;
    }

    public async Task<Result<ProductToReturnDto>> Handle(
        UploadProductImageCommand request,
        CancellationToken cancellationToken
    ) {
        var repo = _unitOfWork.Repository<Domain.Entities.Product>();
        var product = await repo.GetByIdAsync(request.ProductId);
        if (product is null)
            return Result<ProductToReturnDto>.Fail("Product not found", "PRODUCT_NOT_FOUND");

        var pictureUrl = await _pictureService.UploadPictureAsync(request.FileStream, request.FileName);
        product.PictureUrl = pictureUrl;

        repo.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProductToReturnDto>.Ok(_mapper.Map<ProductToReturnDto>(product));
    }
}
