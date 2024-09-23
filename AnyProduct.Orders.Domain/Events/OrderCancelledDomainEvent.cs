using AnyProduct.Orders.Domain.Entities.OrderAggregate;


namespace AnyProduct.Orders.Domain.Events;
public record class OrderCancelledDomainEvent(Order Order) : IDomainEvent;