
using AnyProduct.EventBus.Events;
using AnyProduct.Orders.Application.IntegrationEvents.Models;
using System.Text.Json.Serialization;

namespace AnyProduct.Orders.Application.IntegrationEvents;

public class OrderStartedIntegrationEvent : IntegrationEvent
{
    public string UserId { get; init; }

    public Guid OrderId { get; init; }

    public ICollection<OrderStockItem> OrderStockItems { get; init; }

    [JsonConstructor]
    public OrderStartedIntegrationEvent() { }

    // Ensure PartitionKey to be the same for all events of OrderStartedIntegrationEvent
    // in order to avoid concurrency issues.
    // In this case we pass "DefaultPartitionKey" therefore all the events will be processed in sequence
    // In this case We need Products microsevrice to perform multiple checking and resevation in order set by single Kafka partition
    public OrderStartedIntegrationEvent(string userId, Guid orderId, ICollection<OrderStockItem> orderStockItems) : base("DefaultPartitionKey")
        => (UserId, OrderId, OrderStockItems) = (userId, orderId, orderStockItems);
}
