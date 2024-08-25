
namespace AnyProduct.Orders.Application.Dtos;

public class PaymentDto
{
    public DateTime PaymentDate { get; set; }

    public decimal Price { get; set; }

    public Guid Orderid { get; set; }

    public string BuyerId { get; set; }

    public string PaymentMethodDescription { get; set; }

    public string ExternalPaymentRequestId { get; set; }
}
