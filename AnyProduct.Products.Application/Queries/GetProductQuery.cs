

using AnyProduct.Products.Application.Dtos;
using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AnyProduct.Products.Application.Queries;

public class GetProductQuery : IRequest<ProductDto?>
{
    public Guid Id { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
{
    public readonly IProductRepository _productRepository;
    public readonly IProductCategoryRepository _productCategoryRepository;
    public readonly IConfiguration _configuration;

    public GetProductQueryHandler(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _configuration = configuration;
    }

    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {

        var product = _productRepository.FindById(request.Id);

        if (product is { })
        {
            var categories = _productCategoryRepository.FindManyById(product.ProductCategoryIds);
            return new ProductDto
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
            };
        }


        return null;
    }
}
