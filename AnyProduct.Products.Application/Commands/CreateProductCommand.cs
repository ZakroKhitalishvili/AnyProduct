using AnyProduct.Products.Application.Services;
using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Commands;

public class CreateProductCommand : IRequest<Unit>
{
    public required string Name { get; set; }

    public required int Amount { get; set; }

    public required decimal Price { get; set; }

    public required IFormFile Image { get; set; }

    public required IReadOnlyCollection<Guid> ProductCategoryIds { get; set; }
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

    public async Task<Unit> Handle([NotNull] CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryIdSet = request.ProductCategoryIds.ToHashSet();

        var invalidCategoryId = categoryIdSet.FirstOrDefault(categoryId => _productCategoryRepository.FindById(categoryId) is null);

        if (invalidCategoryId is { })
        {
            throw new InvalidOperationException($"Category Id {invalidCategoryId} is not valid");
        }

        var (_, uniqueName) = await _fileService.UploadAsync(request.Image);

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
