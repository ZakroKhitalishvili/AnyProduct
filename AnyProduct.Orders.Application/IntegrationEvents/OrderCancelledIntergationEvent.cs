
using AnyProduct.EventBus.Events;
using AnyProduct.Orders.Application.IntegrationEvents.Models;

namespace AnyProduct.Orders.Application.IntegrationEvents;

public class OrderCancelledIntergationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public ICollection<OrderStockItem> OrderStockItems { get; init; }

    public OrderCancelledIntergationEvent(Guid orderId, ICollection<OrderStockItem> orderStockItems) : base("DefaultPartitionKey")
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}
