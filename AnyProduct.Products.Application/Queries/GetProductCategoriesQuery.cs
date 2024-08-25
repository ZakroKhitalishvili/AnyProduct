using AnyProduct.Products.Application.Dtos;
using AnyProduct.Products.Domain.Repositories;
using MediatR;

namespace AnyProduct.Products.Application.Queries;

public class GetProductCategorisQuery : IRequest<PagedListDto<ProductCategoryDto>>
{
    public int? Page { get; set; }

    public int? PageSize { get; set; }
}

public class GetProductCategorisQueryHandler : IRequestHandler<GetProductCategorisQuery, PagedListDto<ProductCategoryDto>>
{
    public readonly IProductRepository _productRepository;
    public readonly IProductCategoryRepository _productCategoryRepository;

    public GetProductCategorisQueryHandler(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<PagedListDto<ProductCategoryDto>> Handle(GetProductCategorisQuery request, CancellationToken cancellationToken)
    {
        request.Page ??= 1;
        request.PageSize ??= 10;


        var categories = _productCategoryRepository.GetList(out int totalSize, request.Page.Value, request.PageSize.Value);

        return new PagedListDto<ProductCategoryDto>
        {
            Items = categories.Select(c => new ProductCategoryDto
            {
                Id = c.AggregateId,
                Name = c.Name,
            }).ToList(),
            Page = request.Page.Value,
            PageSize = request.PageSize.Value,
            Total = totalSize,
        };
    }
}
