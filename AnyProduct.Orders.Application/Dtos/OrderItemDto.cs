

namespace AnyProduct.Orders.Application.Dtos;

public class OrderItemDto
{
    public Guid ProductId { get; init; }

    public string? ProductName { get; init; }

    public decimal? UnitPrice { get; init; }

    public int Units { get; init; }

    public string? ImageUrl { get; init; }
}
