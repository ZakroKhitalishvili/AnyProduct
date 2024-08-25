using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;

namespace AnyProduct.Products.Application.Commands;

public class DeleteProductCategoryCommand : IRequest<Unit>
{
    public Guid Id { get; set; }

}

public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Unit>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteProductCategoryCommandHandler(IProductCategoryRepository productRepository, IDateTimeProvider dateTimeProvider)
    {
        _productCategoryRepository = productRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Unit> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = _productCategoryRepository.FindById(request.Id);

        if (productCategory is null)
        {
            throw new Exception("Category not found");
        }

        _productCategoryRepository.Delete(request.Id);

        return Unit.Value;
    }

}
