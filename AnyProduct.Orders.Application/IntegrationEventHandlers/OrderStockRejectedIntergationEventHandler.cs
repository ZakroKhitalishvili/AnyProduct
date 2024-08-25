
using AnyProduct.EventBus.Abstractions;
using AnyProduct.Orders.Application.Commands.Order;
using AnyProduct.Orders.Application.IntegrationEvents;
using MediatR;

namespace AnyProduct.Orders.Application.IntegrationEventHandlers;

public class OrderStockRejectedIntergationEventHandler : IIntegrationEventHandler<OrderStockRejectedIntergationEvent>
{
    private readonly IMediator _mediator;

    public OrderStockRejectedIntergationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(OrderStockRejectedIntergationEvent @event)
    {
        await _mediator.Send(new RejectOrderCommand() { OrderId = @event.OrderId, RejectedProducts = @event.RejectedProducts, OrderStockDetailedItems = @event.OrderStockDetailedItems });
    }
}
