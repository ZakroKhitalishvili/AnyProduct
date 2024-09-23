using System.Text.Json.Serialization;

namespace AnyProduct.Orders.Domain.Entities.OrderAggregate;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    None = 0,
    Started = 1,
    Paid = 2,
    Shipped = 3,
    Cancelled = 4,
    Failed = 5,
}
