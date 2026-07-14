using AutoMapper;
using ECommerceAPI.Application.DTOs.Type;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Types.Commands;

public record CreateTypeCommand(CreateTypeDto Type) : IRequest<Result<TypeToReturnDto>>;
public class CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, Result<TypeToReturnDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TypeToReturnDto>> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
    {
        var type = _mapper.Map<ProductType>(request.Type);

        await _unitOfWork.Repository<ProductType>().AddAsync(type);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TypeToReturnDto>.Ok(_mapper.Map<TypeToReturnDto>(type));
    }
}
