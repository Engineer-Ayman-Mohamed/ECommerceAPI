using FluentValidation;
using ECommerceAPI.Application.DTOs.Type;

namespace ECommerceAPI.Application.Validators;

public class CreateTypeValidator : AbstractValidator<CreateTypeDto>
{
    public CreateTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Type name is required")
            .MaximumLength(50).WithMessage("Type name must not exceed 50 characters");
    }
}
