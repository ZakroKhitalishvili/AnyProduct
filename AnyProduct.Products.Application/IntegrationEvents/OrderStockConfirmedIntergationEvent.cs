
using AnyProduct.EventBus.Events;
using AnyProduct.Products.Application.IntegrationEvents.Models;
using System.Text.Json.Serialization;

namespace AnyProduct.Products.Application.IntegrationEvents;

public class OrderStockConfirmedIntergationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public ICollection<OrderStockDetailedItem> OrderStockDetailedItems { get; init; }

    [JsonConstructor]
    public OrderStockConfirmedIntergationEvent() { }

    public OrderStockConfirmedIntergationEvent(Guid orderId, ICollection<OrderStockDetailedItem> orderStockDetailedItems) : base("DefaultPartitionKey")
    {
        OrderId = orderId;
        OrderStockDetailedItems = orderStockDetailedItems;
    }
}
