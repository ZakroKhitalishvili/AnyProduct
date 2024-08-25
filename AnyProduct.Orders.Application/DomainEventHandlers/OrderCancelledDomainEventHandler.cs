using AnyProduct.Orders.Application.IntegrationEvents;
using AnyProduct.Orders.Domain.Events;
using MediatR;

namespace AnyProduct.Orders.Application.DomainEventHandlers;

public class OrderCancelledDomainEventHandler(IIntegrationEventService integrationEventService) : INotificationHandler<DomainEventNotification<OrderCancelledDomainEvent>>
{
    public async Task Handle(DomainEventNotification<OrderCancelledDomainEvent> notification, CancellationToken cancellationToken)
    {
        await integrationEventService.AddEventAsync(new OrderCancelledIntergationEvent(
            notification.DomainEvent.Order.AggregateId,
            notification.DomainEvent.Order.OrderItems
            .Select(x => new IntegrationEvents.Models.OrderStockItem(x.ProductId, x.Units)).ToArray()
            ));

    }
}
