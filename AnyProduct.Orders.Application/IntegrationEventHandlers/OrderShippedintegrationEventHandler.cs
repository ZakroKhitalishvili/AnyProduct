

using AnyProduct.EventBus.Abstractions;
using AnyProduct.Orders.Application.Commands.Order;
using AnyProduct.Orders.Application.IntegrationEvents;
using MediatR;

namespace AnyProduct.Orders.Application.IntegrationEventHandlers;

public class OrderShippedintegrationEventHandler : IIntegrationEventHandler<OrderShippedIntergationEvent>
{
    private readonly IMediator _mediator;

    public OrderShippedintegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(OrderShippedIntergationEvent @event)
    {
        await _mediator.Send(new SetOrderStatusToShipCommand() { OrderId = @event.OrderId });
    }
}
