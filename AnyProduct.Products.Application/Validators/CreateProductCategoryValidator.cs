using AnyProduct.Products.Application.Commands;
using FluentValidation;

namespace AnyProduct.Products.Application.Validators;

public class CreateProductCategoryValidator : AbstractValidator<CreateProductCategoryCommand>
{

    public CreateProductCategoryValidator()
    {
        RuleFor(product => product.Name)
            .NotNull()
            .MaximumLength(200);
    }
}