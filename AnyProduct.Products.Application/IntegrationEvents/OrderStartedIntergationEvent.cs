
using AnyProduct.EventBus.Events;
using AnyProduct.Products.Application.IntegrationEvents.Models;

namespace AnyProduct.Products.Application.IntegrationEvents;

public class OrderStartedIntegrationEvent : IntegrationEvent
{
    public required string UserId { get; init; }

    public required Guid OrderId { get; init; }

    public required IReadOnlyCollection<OrderStockItem> OrderStockItems { get; init; }
}
