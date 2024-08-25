

namespace AnyProduct.Orders.Application.Dtos;

public class RequestedOrderItemDto
{
    public Guid ProductId { get; init; }

    public int Units { get; init; }
}
