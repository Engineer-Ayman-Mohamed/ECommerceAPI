using FluentValidation;
using ECommerceAPI.Application.DTOs.Order;

namespace ECommerceAPI.Application.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.BasketId)
            .NotEmpty().WithMessage("Basket ID is required");

        RuleFor(x => x.DeliveryMethodId)
            .GreaterThan(0).WithMessage("Delivery method is required");

        RuleFor(x => x.ShippingAddress)
            .NotNull().WithMessage("Shipping address is required")
            .ChildRules(address =>
            {
                address.RuleFor(a => a.FirstName)
                    .NotEmpty().WithMessage("First name is required");
                address.RuleFor(a => a.LastName)
                    .NotEmpty().WithMessage("Last name is required");
                address.RuleFor(a => a.Street)
                    .NotEmpty().WithMessage("Street is required");
                address.RuleFor(a => a.City)
                    .NotEmpty().WithMessage("City is required");
                address.RuleFor(a => a.State)
                    .NotEmpty().WithMessage("State is required");
                address.RuleFor(a => a.ZipCode)
                    .NotEmpty().WithMessage("Zip code is required");
            });
    }
}
