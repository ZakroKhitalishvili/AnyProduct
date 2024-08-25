namespace AnyProduct.Products.Application.IntegrationEvents.Models;

public record OrderStockItem(Guid ProductId, int Units);