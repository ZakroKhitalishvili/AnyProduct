

using AnyProduct.Products.Application.Dtos;
using AnyProduct.Products.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Queries;

public class GetProductsQuery : IRequest<PagedListDto<ProductDto>>
{
    public IReadOnlyCollection<Guid>? CategoryIds { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}

public class GetProductsQueryHander : IRequestHandler<GetProductsQuery, PagedListDto<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IConfiguration _configuration;

    public GetProductsQueryHander(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _configuration = configuration;
    }

    public Task<PagedListDto<ProductDto>> Handle([NotNull] GetProductsQuery request, CancellationToken cancellationToken)
    {
        request.Page ??= 1;
        request.PageSize ??= 10;

        var products = _productRepository.GetList(out int totalSize, request.CategoryIds as ICollection<Guid>, request.Page.Value, request.PageSize.Value);




        var items = new List<ProductDto>();

        foreach (var product in products)
        {
            var categories = _productCategoryRepository.FindManyById(product.ProductCategoryIds);

            items.Add(new ProductDto
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
            });

        }

        var result = new PagedListDto<ProductDto>()
        {
            Items = items,
            Page = request.Page.Value,
            PageSize = request.PageSize.Value,
            Total = totalSize,
        };


        return Task.FromResult(result);
    }
}
