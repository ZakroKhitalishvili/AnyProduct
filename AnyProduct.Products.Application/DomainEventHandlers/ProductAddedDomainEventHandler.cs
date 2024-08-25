
using AnyProduct.Products.Application.IntegrationEvents;
using AnyProduct.Products.Domain.Events;
using MediatR;

namespace AnyProduct.Products.Application.DomainEventHandlers;

public class ProductAddedDomainEventHandler : INotificationHandler<DomainEventNotification<ProductAddedDomainEvent>>
{
    private readonly IIntegrationEventService _integrationEventService;

    public ProductAddedDomainEventHandler(IIntegrationEventService integrationEventService)
    {
        _integrationEventService = integrationEventService;
    }

    public async Task Handle(DomainEventNotification<ProductAddedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        //await _integrationEventService.AddEventAsync(new ProductAddedIntegrationEvent(string.Empty, domainEvent.Product.AggregateId, domainEvent.Product.Name));
    }
}
