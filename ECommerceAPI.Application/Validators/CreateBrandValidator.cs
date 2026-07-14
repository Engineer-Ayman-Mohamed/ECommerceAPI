using FluentValidation;
using ECommerceAPI.Application.DTOs.Brand;

namespace ECommerceAPI.Application.Validators;

public class CreateBrandValidator : AbstractValidator<CreateBrandDto>
{
    public CreateBrandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Brand name is required")
            .MaximumLength(50).WithMessage("Brand name must not exceed 50 characters");
    }
}
