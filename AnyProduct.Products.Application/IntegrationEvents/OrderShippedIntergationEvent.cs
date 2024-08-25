
using AnyProduct.EventBus.Events;

namespace AnyProduct.Products.Application.IntegrationEvents;

public class OrderShippedIntergationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public OrderShippedIntergationEvent(Guid orderId) : base("DefaultPartitionKey")
        => OrderId = orderId;
}
