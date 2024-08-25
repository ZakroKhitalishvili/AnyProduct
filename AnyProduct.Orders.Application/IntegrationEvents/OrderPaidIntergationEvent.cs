
using AnyProduct.EventBus.Events;
using AnyProduct.Orders.Application.IntegrationEvents.Models;

namespace AnyProduct.Orders.Application.IntegrationEvents;

public class OrderPaidIntergationEvent : IntegrationEvent
{

    public Guid OrderId { get; init; }

    public ICollection<OrderStockItem> OrderStockItems { get; init; }
    // Ensure PartitionKey to be the same for all events of OrderStartedIntegrationEvent
    // in order to avoid concurrency issues.
    // In this case we pass "DefaultPartitionKey" therefore all the events will be processed in sequence
    // In this case We need Products microsevrice to perform multiple checking and resevation in order set by single Kafka partition
    public OrderPaidIntergationEvent(Guid orderId, ICollection<OrderStockItem> orderStockItems) : base("DefaultPartitionKey")
        => (OrderId, OrderStockItems) = ( orderId, orderStockItems);
}
