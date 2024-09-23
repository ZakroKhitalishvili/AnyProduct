

using AnyProduct.Orders.Domain.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Application.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }

    public DateTime OrderDate { get; set; }

    public required ShippingAddressDto Address { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public required string Description { get; set; }

    public required IReadOnlyCollection<OrderItemDto> OrderItems { get; set; }

    public static OrderDto From(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return new OrderDto
        {
            Id = order.AggregateId,
            Address = new ShippingAddressDto
            {
                City = order.Address.City,
                Country = order.Address.Country,
                State = order.Address.State,
                Street = order.Address.Street,
                ZipCode = order.Address.ZipCode,
            },
            Description = order.Description,
            OrderDate = order.OrderDate,
            OrderStatus = order.OrderStatus,
            OrderItems = order.OrderItems.Select(x =>
            new OrderItemDto
            {
                ImageUrl = x.ImageUrl,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Units = x.Units,
            }).ToArray(),
        };
    }
}
