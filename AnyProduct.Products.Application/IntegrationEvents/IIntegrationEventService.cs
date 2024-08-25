using AnyProduct.EventBus.Events;

namespace AnyProduct.Products.Application.IntegrationEvents;

public interface IIntegrationEventService
{
    Task PublishEventsAsync(string transactionId);
    Task AddEventAsync(IntegrationEvent evt);
}
