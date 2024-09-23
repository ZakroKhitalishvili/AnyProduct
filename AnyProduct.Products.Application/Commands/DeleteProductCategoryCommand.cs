using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Commands;

public class DeleteProductCategoryCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Unit>
{
    private readonly IProductCategoryRepository _productCategoryRepository;

    public DeleteProductCategoryCommandHandler(IProductCategoryRepository productRepository, IDateTimeProvider dateTimeProvider)
    {
        _productCategoryRepository = productRepository;
    }

    public Task<Unit> Handle([NotNull] DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = _productCategoryRepository.FindById(request.Id);

        if (productCategory is null)
        {
            throw new InvalidOperationException("Category not found");
        }

        _productCategoryRepository.Delete(request.Id);

        return Task.FromResult(Unit.Value);
    }

}
