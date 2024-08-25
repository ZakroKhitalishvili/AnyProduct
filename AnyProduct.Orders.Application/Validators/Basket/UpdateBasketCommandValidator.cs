
using AnyProduct.Orders.Application.Commands.Basket;
using FluentValidation;

namespace AnyProduct.Orders.Application.Validators.Basket;

public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
{
    public UpdateBasketCommandValidator()
    {
        RuleFor(product => product.BasketItems)
            .NotEmpty()
            .WithMessage("Basket must have at least one item");
    }
}
