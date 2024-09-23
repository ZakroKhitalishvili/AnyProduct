using AnyProduct.EventBus.Abstractions;
using AnyProduct.Products.Application.Commands;
using AnyProduct.Products.Application.IntegrationEvents;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.IntegrationEventHandlers;

public class OrderCancelledIntergationEventHandler(IMediator mediator) : IIntegrationEventHandler<OrderCancelledIntergationEvent>
{
    public async Task Handle([NotNull] OrderCancelledIntergationEvent @event)
    {
        await mediator.Send(new RemoveReservationCommand()
        {
            OrderId = @event.OrderId,
            OrderStockItems = @event.OrderStockItems,
        });

    }
}
