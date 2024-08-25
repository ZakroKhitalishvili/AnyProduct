using AnyProduct.Orders.Application.Commands.Order;
using AnyProduct.Orders.Application.Dtos;
using AnyProduct.Orders.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnyProduct.Orders.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpGet("{Id}")]
    public Task<OrderDto?> GetOrder([FromRoute] GetSingleOrderQuery query)
    {
        return mediator.Send(query);
    }

    [HttpPost(nameof(PlaceOrder))]
    public async Task<PlacedOrderDto> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        return await mediator.Send(command);
    }

    [HttpPost(nameof(CancelOrder))]
    public async Task<IActionResult> CancelOrder([FromBody] CancelOrderCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpGet(nameof(OrderHistory))]
    public Task<PagedListDto<OrderDto>> OrderHistory([FromQuery] GetOrderQuery query)
    {
        return mediator.Send(query);
    }

    [HttpGet(nameof(CustomerOrderHistory))]
    public Task<PagedListDto<OrderDto>> CustomerOrderHistory([FromQuery] GetCustomerOrdersQuery query)
    {
        return mediator.Send(query);
    }

    [HttpGet(nameof(PaymentHistory))]
    public Task<PagedListDto<PaymentDto>> PaymentHistory([FromQuery] GetPaymentQuery query)
    {
        return mediator.Send(query);
    }

    [HttpGet(nameof(CustomerPaymentHistory))]
    public Task<PagedListDto<PaymentDto>> CustomerPaymentHistory([FromQuery] GetCustomerPaymentsQuery query)
    {
        return mediator.Send(query);
    }
}
