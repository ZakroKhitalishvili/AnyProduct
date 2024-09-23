using AnyProduct.Orders.Domain.Entities.OrderAggregate;

namespace AnyProduct.Orders.Domain.Events;
public record class OrderPaidDomainEvent(Guid OrderId, ICollection<OrderItem> OrderItems) : IDomainEvent;