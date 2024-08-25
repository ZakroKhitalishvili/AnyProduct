using AnyProduct.Products.Application.Commands;
using FluentValidation;

public class UpdateProductCategoryValidator : AbstractValidator<CreateProductCategoryCommand>
{

    public UpdateProductCategoryValidator()
    {
        RuleFor(product => product.Name)
            .NotNull()
            .MaximumLength(200);
    }
}