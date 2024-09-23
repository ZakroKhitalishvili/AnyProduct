
using AnyProduct.EventBus.Abstractions;
using AnyProduct.Products.Application.Commands;
using AnyProduct.Products.Application.IntegrationEvents;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.IntegrationEventHandlers;

public class OrderStartedIntergationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public OrderStartedIntergationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle([NotNull] OrderStartedIntegrationEvent @event)
    {
        await _mediator.Send(new CheckAndReserveProductStockCommand() { OrderId = @event.OrderId, OrderStockItems = @event.OrderStockItems });

    }
}
