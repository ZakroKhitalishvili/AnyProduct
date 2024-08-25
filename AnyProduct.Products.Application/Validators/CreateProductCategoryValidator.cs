using AnyProduct.Products.Application.Commands;
using FluentValidation;

public class CreateProductCategoryValidator : AbstractValidator<CreateProductCategoryCommand>
{

    public CreateProductCategoryValidator()
    {
        RuleFor(product => product.Name)
            .NotNull()
            .MaximumLength(200);
    }
}