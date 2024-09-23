using AnyProduct.Products.Application.Commands;
using FluentValidation;

namespace AnyProduct.Products.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    private const int ImageMaxSizeInBytes = 5_242_880;// 5mb

    public CreateProductValidator()
    {
        RuleFor(product => product.Name)
            .NotNull()
            .MaximumLength(200);

        RuleFor(product => product.Price)
            .NotEmpty()
            .Must(price => price > 0);

        RuleFor(product => product.Amount)
            .NotEmpty()
            .Must(price => price >= 0)
            .WithMessage("Price must be non-negative number");

        RuleFor(product => product.ProductCategoryIds)
            .NotEmpty()
            .WithMessage("Product must have at least one category");

        RuleFor(product => product.Image)
            .Must(image => image.Length <= ImageMaxSizeInBytes)
            .WithMessage("Image's size exceeds 5 mb");
    }
}