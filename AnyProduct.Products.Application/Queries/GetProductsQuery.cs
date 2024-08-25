

using AnyProduct.Products.Application.Dtos;
using AnyProduct.Products.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AnyProduct.Products.Application.Queries;

public class GetProductsQuery : IRequest<PagedListDto<ProductDto>>
{
    public ICollection<Guid>? CategoryIds { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}

public class GetProductsQueryHander : IRequestHandler<GetProductsQuery, PagedListDto<ProductDto>>
{
    public readonly IProductRepository _productRepository;
    public readonly IProductCategoryRepository _productCategoryRepository;
    public readonly IConfiguration _configuration;

    public GetProductsQueryHander(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _configuration = configuration;
    }

    public async Task<PagedListDto<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        request.Page ??= 1;
        request.PageSize ??= 10;

        var products = _productRepository.GetList(out int totalSize, request.CategoryIds, request.Page.Value, request.PageSize.Value);


        var result = new PagedListDto<ProductDto>()
        {
            Items = new List<ProductDto>(),
            Page = request.Page.Value,
            PageSize = request.PageSize.Value,
            Total = totalSize,
        };

        foreach (var product in products)
        {
            var categories = _productCategoryRepository.FindManyById(product.ProductCategoryIds);

            result.Items.Add(new ProductDto
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


        return result;
    }
}
