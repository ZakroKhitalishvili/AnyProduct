
namespace AnyProduct.Orders.Application.Dtos;

public class PaymentDto
{
    public required DateTime PaymentDate { get; set; }

    public required decimal Price { get; set; }

    public required Guid Orderid { get; set; }

    public required string BuyerId { get; set; }

    public required string PaymentMethodDescription { get; set; }

    public required string ExternalPaymentRequestId { get; set; }
}
