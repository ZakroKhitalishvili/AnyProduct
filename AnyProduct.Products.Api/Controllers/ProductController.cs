using AnyProduct.Products.Application.Commands;
using AnyProduct.Products.Application.Dtos;
using AnyProduct.Products.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnyProduct.Products.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<PagedListDto<ProductDto>> GetProducts([FromQuery] GetProductsQuery query)
        {
            return await _sender.Send(query);
        }

        [HttpGet("{Id:Guid}")]
        public async Task<ProductDto?> GetProduct([FromRoute] GetProductQuery query)
        {
            return await _sender.Send(query);
        }

        [HttpPost]
        public async Task Create([FromForm] CreateProductCommand command)
        {
            await _sender.Send(command);
        }

        [HttpPut]
        public async Task Update([FromForm] UpdateProductCommand command)
        {
            await _sender.Send(command);
        }

        [HttpDelete]
        public async Task Delete([FromForm] DeleteProductCommand command)
        {
            await _sender.Send(command);
        }
    }
}
