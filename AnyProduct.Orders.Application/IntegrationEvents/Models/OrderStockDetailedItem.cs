namespace AnyProduct.Orders.Application.IntegrationEvents.Models;

public record OrderStockDetailedItem(Guid ProductId, string ProductName, Uri ImageUrl, decimal UniPrice, int Units);