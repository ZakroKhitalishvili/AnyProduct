using AnyProduct.EventBus.Events;

namespace AnyProduct.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);
}
