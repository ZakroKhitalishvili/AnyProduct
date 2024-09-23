using AnyProduct.Orders.Domain;
using AnyProduct.Orders.Domain.Entities.BuyerAggregate;

namespace AnyProduct.Orders.Domain.Events;

public record PaymentMethodVerifiedDomainEvent : IDomainEvent
{
    public Buyer Buyer { get; private set; }
    public PaymentMethod Payment { get; private set; }
    public Guid OrderId { get; private set; }

    public PaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod payment, Guid orderId)
    {
        Buyer = buyer;
        Payment = payment;
        OrderId = orderId;
    }
}