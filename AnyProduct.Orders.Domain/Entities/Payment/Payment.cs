
namespace AnyProduct.Orders.Domain.Entities.Balance;

public class Payment : AggregateRoot
{
    public Payment() { }
    public Payment(DateTime paymentDate, decimal price, Guid orderid, string buyerId, string externalPaymentId, string paymentMethodDescription)
    {
        AggregateId = Guid.NewGuid();
        PaymentDate = paymentDate;
        Price = price;
        Orderid = orderid;
        BuyerId = buyerId;
        ExternalPaymentRequestId = externalPaymentId;
        PaymentMethodDescription = paymentMethodDescription;
    }

    public DateTime PaymentDate { get; private set; }

    public decimal Price { get; private set; }

    public Guid Orderid { get; private set; }

    public string BuyerId { get; private set; }

    public string PaymentMethodDescription { get; private set; }

    public string ExternalPaymentRequestId { get; private set; }
}
