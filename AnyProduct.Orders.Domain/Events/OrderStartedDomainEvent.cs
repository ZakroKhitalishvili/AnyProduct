
using AnyProduct.Orders.Domain.Entities.Buyer;
using AnyProduct.Orders.Domain.Entities.Order;

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
