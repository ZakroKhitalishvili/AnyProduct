using AnyProduct.Orders.Domain.Entities.Order;


namespace AnyProduct.Orders.Domain.Events;
public record class OrderShippedDomainEvent(Order Order) : IDomainEvent;