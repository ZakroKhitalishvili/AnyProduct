using AnyProduct.Products.Application.Commands;
using FluentValidation;

namespace AnyProduct.Products.Application.Validators;

public class UpdateProductCategoryValidator : AbstractValidator<CreateProductCategoryCommand>
{

    public UpdateProductCategoryValidator()
    {
        RuleFor(product => product.Name)
            .NotNull()
            .MaximumLength(200);
    }
}