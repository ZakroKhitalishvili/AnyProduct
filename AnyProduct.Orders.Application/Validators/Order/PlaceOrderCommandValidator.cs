
using AnyProduct.Orders.Application.Commands.Basket;
using AnyProduct.Orders.Application.Commands.Order;
using FluentValidation;

namespace AnyProduct.Orders.Application.Validators.Basket;

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {

        RuleFor(product => product.OrderItems)
            .NotEmpty()
            .WithMessage("Order must have at least one item");

        RuleFor(product => product.ShippingAddress)
            .NotEmpty();

        RuleFor(product => product.CardData)
            .NotEmpty();

        RuleFor(product => product.UserName)
            .NotEmpty();

    }
}
