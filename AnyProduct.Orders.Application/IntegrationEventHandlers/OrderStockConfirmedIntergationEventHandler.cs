

using AnyProduct.EventBus.Abstractions;
using AnyProduct.Orders.Application.Commands.Order;
using AnyProduct.Orders.Application.IntegrationEvents;
using MediatR;

namespace AnyProduct.Orders.Application.IntegrationEventHandlers;

public class OrderStockConfirmedIntergationEventHandler : IIntegrationEventHandler<OrderStockConfirmedIntergationEvent>
{
    private readonly IMediator _mediator;

    public OrderStockConfirmedIntergationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(OrderStockConfirmedIntergationEvent @event)
    {
        await _mediator.Send(new ExecuteOrderCommand() { OrderId = @event.OrderId, OrderStockDetailedItems = @event.OrderStockDetailedItems });
    }
}
