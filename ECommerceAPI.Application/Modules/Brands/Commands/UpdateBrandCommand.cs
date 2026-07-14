using AutoMapper;
using ECommerceAPI.Application.DTOs.Brand;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Brands.Commands;

public record UpdateBrandCommand(int Id, CreateBrandDto Brand) : IRequest<Result<BrandToReturnDto>>;
public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Result<BrandToReturnDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BrandToReturnDto>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(request.Id);

        if (brand is null)
            return Result<BrandToReturnDto>.Fail($"Brand with ID {request.Id} not found", ErrorCodes.BrandNotFound);

        brand.Name = request.Brand.Name;
        _unitOfWork.Repository<ProductBrand>().Update(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<BrandToReturnDto>.Ok(_mapper.Map<BrandToReturnDto>(brand));
    }
}
