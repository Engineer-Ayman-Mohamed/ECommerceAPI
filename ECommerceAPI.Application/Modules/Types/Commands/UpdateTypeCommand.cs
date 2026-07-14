using AutoMapper;
using ECommerceAPI.Application.DTOs.Type;
using ECommerceAPI.Application.Shared;
using ECommerceAPI.Application.Shared.Errors;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using MediatR;

namespace ECommerceAPI.Application.Modules.Types.Commands;

public record UpdateTypeCommand(int Id, CreateTypeDto Type) : IRequest<Result<TypeToReturnDto>>;
public class UpdateTypeCommandHandler : IRequestHandler<UpdateTypeCommand, Result<TypeToReturnDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TypeToReturnDto>> Handle(UpdateTypeCommand request, CancellationToken cancellationToken)
    {
        var type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(request.Id);

        if (type is null)
            return Result<TypeToReturnDto>.Fail($"Type with ID {request.Id} not found", ErrorCodes.TypeNotFound);

        type.Name = request.Type.Name;
        _unitOfWork.Repository<ProductType>().Update(type);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TypeToReturnDto>.Ok(_mapper.Map<TypeToReturnDto>(type));
    }
}
