

using AnyProduct.Orders.Domain.Entities.Order;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Application.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }

    public DateTime OrderDate { get; set; }

    public ShippingAddressDto Address { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public string Description { get; set; }

    public ICollection<OrderItemDTO> OrderItems { get; set; }

    public static OrderDto From(Order order)
    {
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
            new OrderItemDTO
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
