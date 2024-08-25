
using AnyProduct.EventBus.Abstractions;
using AnyProduct.Products.Application.Commands;
using AnyProduct.Products.Application.IntegrationEvents;
using MediatR;

namespace AnyProduct.Products.Application.IntegrationEventHandlers;

public class OrderPaidIntergationEventHandler : IIntegrationEventHandler<OrderPaidIntergationEvent>
{
    private readonly IMediator _mediator;

    public OrderPaidIntergationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(OrderPaidIntergationEvent @event)
    {
        await _mediator.Send(new PrepareProductShipmentCommand() { OrderId = @event.OrderId, OrderStockItems = @event.OrderStockItems });
    }
}
