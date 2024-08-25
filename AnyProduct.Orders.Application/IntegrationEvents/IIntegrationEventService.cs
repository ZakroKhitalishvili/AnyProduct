using AnyProduct.EventBus.Events;

namespace AnyProduct.Orders.Application.IntegrationEvents;

public interface IIntegrationEventService
{
    Task PublishEventsAsync(string transactionId);
    Task AddEventAsync(IntegrationEvent evt);
}
