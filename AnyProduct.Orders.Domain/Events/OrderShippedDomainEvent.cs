using AnyProduct.Orders.Domain.Entities.OrderAggregate;


namespace AnyProduct.Orders.Domain.Events;
public record class OrderShippedDomainEvent(Order Order) : IDomainEvent;