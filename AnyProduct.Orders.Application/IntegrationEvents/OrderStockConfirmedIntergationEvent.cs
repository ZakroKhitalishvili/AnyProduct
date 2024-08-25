
using AnyProduct.EventBus.Events;
using AnyProduct.Orders.Application.IntegrationEvents.Models;

namespace AnyProduct.Orders.Application.IntegrationEvents;

public class OrderStockConfirmedIntergationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public ICollection<OrderStockDetailedItem> OrderStockDetailedItems { get; init; }
}
