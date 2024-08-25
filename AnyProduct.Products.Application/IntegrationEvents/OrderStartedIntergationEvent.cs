
using AnyProduct.EventBus.Events;
using AnyProduct.Products.Application.IntegrationEvents.Models;

namespace AnyProduct.Products.Application.IntegrationEvents;

public class OrderStartedIntegrationEvent : IntegrationEvent
{
    public string UserId { get; init; }

    public Guid OrderId { get; init; }

    public ICollection<OrderStockItem> OrderStockItems { get; init; }
}
