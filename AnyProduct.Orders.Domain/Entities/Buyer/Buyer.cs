using AnyProduct.Orders.Domain.Events;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Domain.Entities.Buyer;

public class Buyer
    : AggregateRoot
{
    [Required]
    public string IdentityId { get; private set; }

    public string Name { get; private set; }

    private List<PaymentMethod> _paymentMethods;

    public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    protected Buyer()
    {
        _paymentMethods = new List<PaymentMethod>();
        AggregateId = Guid.NewGuid();
    }

    public Buyer(string identity, string name) : this()
    {
        IdentityId = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
    }

    public PaymentMethod VerifyOrAddPaymentMethod(
        CardType cardType, string alias, string cardNumber,
        string securityNumber, string cardHolderName, DateTime expiration, Guid orderId)
    {
        var existingPayment = _paymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardType, cardNumber, expiration));

        if (existingPayment != null)
        {
            AddEvent(new PaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));

            return existingPayment;
        }

        var payment = new PaymentMethod(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration);

        _paymentMethods.Add(payment);

        AddEvent(new PaymentMethodVerifiedDomainEvent(this, payment, orderId));

        return payment;
    }
}
