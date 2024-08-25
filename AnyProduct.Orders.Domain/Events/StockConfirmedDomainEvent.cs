
namespace AnyProduct.Orders.Domain.Events;

public record class StockConfirmedDomainEvent(Guid OrderId) : IDomainEvent;
