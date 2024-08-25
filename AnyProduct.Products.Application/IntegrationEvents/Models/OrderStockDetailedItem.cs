﻿namespace AnyProduct.Products.Application.IntegrationEvents.Models;

public record OrderStockDetailedItem(Guid ProductId, string ProductName, string ImageUrl, decimal UniPrice, int Units);