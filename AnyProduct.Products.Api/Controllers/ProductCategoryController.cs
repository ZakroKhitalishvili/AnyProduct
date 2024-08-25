using AnyProduct.Products.Application.Commands;
using AnyProduct.Products.Application.Dtos;
using AnyProduct.Products.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnyProduct.Products.Api.Controllers
{
    [Route("api/product-category")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductCategoryController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<PagedListDto<ProductCategoryDto>> GetProductCategories([FromQuery] GetProductCategorisQuery query)
        {
            return await _sender.Send(query);
        }

        [HttpPost]
        public async Task Create([FromForm] CreateProductCategoryCommand command)
        {
            await _sender.Send(command);
        }

        [HttpPut]
        public async Task Update([FromForm] UpdateProductCategoryCommand command)
        {
            await _sender.Send(command);
        }

        [HttpDelete]
        public async Task Delete([FromForm] DeleteProductCategoryCommand command)
        {
            await _sender.Send(command);
        }
    }
}
