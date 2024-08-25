namespace AnyProduct.Orders.Application.IntegrationEvents.Models;

public record OrderStockItem(Guid ProductId, int Units);