using AnyProduct.Products.Application.Services;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AnyProduct.Products.Application.Commands;

public class UpdateProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Amount { get; set; }

    public decimal Price { get; set; }

    public IFormFile? Image { get; set; }

    public ICollection<Guid> ProductCategoryIds { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IFileService _fileService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateProductCommandHandler(IProductRepository productRepository, IFileService fileService, IDateTimeProvider dateTimeProvider, IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _fileService = fileService;
        _dateTimeProvider = dateTimeProvider;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryIdSet = request.ProductCategoryIds.ToHashSet();

        foreach (var categoryId in categoryIdSet)
        {
            if (_productCategoryRepository.FindById(categoryId) is null)
            {
                throw new Exception($"Category Id {categoryId} is not valid");
            }
        }

        var product = _productRepository.FindById(request.Id);

        if (product is null)
            throw new Exception("Product not found");

        string image = product.Image;

        if (request.Image is not null)
        {
            var (originalName, uniqueName) = await _fileService.UploadAsync(request.Image);

            await _fileService.DeleteAsync(product.Image);

            image = uniqueName;
        }

        product.Update(
            request.Name,
            request.Amount,
            request.Price,
            image,
            _dateTimeProvider.Now,
            categoryIdSet.ToArray()
            );


        _productRepository.Update(product);

        return Unit.Value;
    }

}
