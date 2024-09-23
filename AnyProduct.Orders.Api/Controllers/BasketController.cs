using AnyProduct.Orders.Application.Commands.Basket;
using AnyProduct.Orders.Application.Dtos.Basket;
using AnyProduct.Orders.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AnyProduct.Orders.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketController(IMediator mediator) : ControllerBase
{
    [HttpDelete]
    public async Task<IActionResult> DeleteBasket()
    {
        await mediator.Send(new DeleteBasketCommand());
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBasket([FromBody] UpdateBasketCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpGet]
    public Task<ICollection<BasketItemDto>> GetBasket()
    {
        return mediator.Send(new GetBasketQuery());
    }

}
