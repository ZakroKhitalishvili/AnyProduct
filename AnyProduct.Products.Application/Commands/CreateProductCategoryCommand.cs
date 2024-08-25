using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;

namespace AnyProduct.Products.Application.Commands;

public class CreateProductCategoryCommand : IRequest<Unit>
{
    public string Name { get; set; }

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

    public async Task<Unit> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        if (_productCategoryRepository.ExistName(request.Name))
        {
            throw new Exception("Name is already used");
        }

        var productCategory = new ProductCategory(
            request.Name,
            _dateTimeProvider.Now
            );

        _productCategoryRepository.Add(productCategory);

        return Unit.Value;
    }

}
