using AnyProduct.Products.Application.Services;
using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AnyProduct.Products.Application.Commands;

public class CreateProductCommand : IRequest<Unit>
{
    public string Name { get; set; }

    public int Amount { get; set; }

    public decimal Price { get; set; }

    public IFormFile Image { get; set; }

    public ICollection<Guid> ProductCategoryIds { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IFileService _fileService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateProductCommandHandler(IProductRepository productRepository, IFileService fileService, IDateTimeProvider dateTimeProvider, IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _fileService = fileService;
        _dateTimeProvider = dateTimeProvider;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryIdSet = request.ProductCategoryIds.ToHashSet();

        foreach (var categoryId in categoryIdSet)
        {
            if(_productCategoryRepository.FindById(categoryId) is null)
            {
                throw new Exception($"Category Id {categoryId} is not valid");
            }
        }

        var (originalName, uniqueName) = await _fileService.UploadAsync(request.Image);

        var product = new Product(
            request.Name,
            request.Amount,
            request.Price,
            uniqueName,
            _dateTimeProvider.Now,
            categoryIdSet.ToArray()
            );

        _productRepository.Add(product);

        return Unit.Value;
    }

}
