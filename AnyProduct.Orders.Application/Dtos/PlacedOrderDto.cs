

using AnyProduct.Orders.Domain.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Application.Dtos;

public class PlacedOrderDto
{
    public Guid Id { get; set; }

    public DateTime OrderDate { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public string Description { get; set; }

    public static PlacedOrderDto From(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return new PlacedOrderDto
        {
            Id = order.AggregateId,
            Description = order.Description,
            OrderDate = order.OrderDate,
            OrderStatus = order.OrderStatus,
        };
    }
}
