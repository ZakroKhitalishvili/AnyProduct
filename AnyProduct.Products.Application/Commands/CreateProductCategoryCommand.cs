using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Commands;

public class CreateProductCategoryCommand : IRequest<Unit>
{
    public required string Name { get; set; }

}

public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, Unit>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateProductCategoryCommandHandler(IProductCategoryRepository productRepository, IDateTimeProvider dateTimeProvider)
    {
        _productCategoryRepository = productRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task<Unit> Handle([NotNull] CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        if (_productCategoryRepository.ExistName(request.Name))
        {
            throw new InvalidOperationException("Name is already used");
        }

        var productCategory = new ProductCategory(
            request.Name,
            _dateTimeProvider.Now
            );

        _productCategoryRepository.Add(productCategory);

        return Task.FromResult(Unit.Value);
    }

}
