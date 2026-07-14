using FluentValidation;
using ECommerceAPI.Application.DTOs.Product;

namespace ECommerceAPI.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required")
            .MaximumLength(500).WithMessage("Product description must not exceed 500 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.PictureUrl)
            .NotEmpty().WithMessage("Picture URL is required");

        RuleFor(x => x.ProductTypeId)
            .GreaterThan(0).WithMessage("Product type is required");

        RuleFor(x => x.ProductBrandId)
            .GreaterThan(0).WithMessage("Product brand is required");
    }
}
