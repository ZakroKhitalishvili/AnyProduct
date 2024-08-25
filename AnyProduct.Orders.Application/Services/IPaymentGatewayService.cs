
using AnyProduct.Orders.Domain.Entities.Buyer;
using AnyProduct.Orders.Domain.Entities.Order;

namespace AnyProduct.Orders.Application.Services;

public interface IPaymentGatewayService
{
    Task<string> CreateRequest(Order order, PaymentMethod paymentMethod, string webhookUrl);
    Task<bool> Execute(string requestId);
    Task<bool> Refund(string requestId, string webhookUrl);
}
