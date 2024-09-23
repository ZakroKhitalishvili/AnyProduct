
using AnyProduct.EventBus.Events;
using AnyProduct.Products.Application.IntegrationEvents.Models;

namespace AnyProduct.Products.Application.IntegrationEvents;

public class OrderPaidIntergationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }

    public required IReadOnlyCollection<OrderStockItem> OrderStockItems { get; init; }

}
