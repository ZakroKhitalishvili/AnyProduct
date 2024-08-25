
using AnyProduct.EventBus.Events;
using AnyProduct.Products.Application.IntegrationEvents.Models;
using System.Text.Json.Serialization;

namespace AnyProduct.Products.Application.IntegrationEvents;

public class OrderStockRejectedIntergationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public ICollection<Guid> RejectedProducts { get; init; }

    public ICollection<OrderStockDetailedItem> OrderStockDetailedItems { get; init; }

    [JsonConstructor]
    public OrderStockRejectedIntergationEvent() { }

    public OrderStockRejectedIntergationEvent(Guid orderId, ICollection<Guid> rejectedProducts, ICollection<OrderStockDetailedItem> orderStockDetailedItems) : base(orderId.ToString())
    {
        OrderId = orderId;
        RejectedProducts = rejectedProducts;
        OrderStockDetailedItems = orderStockDetailedItems;
    }
}
