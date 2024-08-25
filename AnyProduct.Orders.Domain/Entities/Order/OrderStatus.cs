using System.Text.Json.Serialization;

namespace AnyProduct.Orders.Domain.Entities.Order;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Started = 1,
    Paid = 2,
    Shipped = 3,
    Cancelled = 4,
    Failed = 5,
}
