using AutoMapper;
using ECommerceAPI.Application.DTOs.Brand;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Brands.Commands;

public record CreateBrandCommand(CreateBrandDto Brand) : IRequest<Result<BrandToReturnDto>>;
public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Result<BrandToReturnDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BrandToReturnDto>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = _mapper.Map<ProductBrand>(request.Brand);

        await _unitOfWork.Repository<ProductBrand>().AddAsync(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<BrandToReturnDto>.Ok(_mapper.Map<BrandToReturnDto>(brand));
    }
}
