
using AnyProduct.Orders.Domain.Entities.BuyerAggregate;
using AnyProduct.Orders.Domain.Entities.OrderAggregate;

namespace AnyProduct.Orders.Domain.Events;

public record class OrderStartedDomainEvent(
    Order Order,
    string UserId,
    string UserName,
    CardType CardType,
    string CardNumber,
    string CardSecurityNumber,
    string CardHolderName,
    DateTime CardExpiration) : IDomainEvent;
