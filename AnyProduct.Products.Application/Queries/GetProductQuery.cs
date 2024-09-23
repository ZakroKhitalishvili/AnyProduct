

using AnyProduct.Products.Application.Dtos;
using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Queries;

public class GetProductQuery : IRequest<ProductDto?>
{
    public Guid Id { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IConfiguration _configuration;

    public GetProductQueryHandler(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _configuration = configuration;
    }

    public Task<ProductDto?> Handle([NotNull] GetProductQuery request, CancellationToken cancellationToken)
    {

        var product = _productRepository.FindById(request.Id);

        if (product is { })
        {
            var categories = _productCategoryRepository.FindManyById(product.ProductCategoryIds);
            var productDto = new ProductDto
            {
                Id = product.AggregateId,
                ImagePath = $"{_configuration["FileUpload:BaseUrl"]}/{product.Image}",
                IsInStock = product.Amount > 0,
                UnitsInStock = product.Amount,
                Name = product.Name,
                Price = product.Price,
                ProductCategories = categories.Select(c => new ProductCategoryDto
                {
                    Id = c.AggregateId,
                    Name = c.Name,
                }).ToList(),
            }!;

            return Task.FromResult<ProductDto?>(productDto);
        }


        return Task.FromResult<ProductDto?>(null);
    }
}
