using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Commands;

public class UpdateProductCategoryCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }

}

public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, Unit>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateProductCategoryCommandHandler(IProductCategoryRepository productRepository, IDateTimeProvider dateTimeProvider)
    {
        _productCategoryRepository = productRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task<Unit> Handle([NotNull] UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = _productCategoryRepository.FindById(request.Id);

        if (productCategory is null)
        {
            throw new InvalidOperationException("Category not found");
        }

        if (productCategory.Name == request.Name)
        {
            throw new InvalidOperationException("Category's name is not different");
        }

        if (_productCategoryRepository.ExistName(request.Name))
        {
            throw new InvalidOperationException("Name is already used");
        }

        productCategory.Update(request.Name, _dateTimeProvider.Now);

        _productCategoryRepository.Update(productCategory);

        return Task.FromResult(Unit.Value);
    }

}
