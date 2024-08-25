
using AnyProduct.EventBus.Events;

namespace AnyProduct.Orders.Application.IntegrationEvents;

public class OrderShippedIntergationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

}
