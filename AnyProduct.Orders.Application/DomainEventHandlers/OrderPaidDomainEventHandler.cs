
using AnyProduct.Orders.Application.IntegrationEvents;
using AnyProduct.Orders.Application.IntegrationEvents.Models;
using AnyProduct.Orders.Domain.Events;
using MediatR;

namespace AnyProduct.Orders.Application.DomainEventHandlers;

public class OrderPaidDomainEventHandler : INotificationHandler<DomainEventNotification<OrderPaidDomainEvent>>
{
    private readonly IIntegrationEventService _integrationEventService;

    public OrderPaidDomainEventHandler(IIntegrationEventService integrationEventService)
    {
        _integrationEventService = integrationEventService;
    }

    public async Task Handle(DomainEventNotification<OrderPaidDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _integrationEventService.AddEventAsync(
            new OrderPaidIntergationEvent(
                notification.DomainEvent.OrderId,
                notification.DomainEvent.OrderItems.Select(x => new OrderStockItem(x.ProductId, x.Units)).ToArray()
            ));
    }
}
