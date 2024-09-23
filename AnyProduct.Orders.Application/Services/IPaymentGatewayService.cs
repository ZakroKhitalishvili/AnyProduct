
using AnyProduct.Orders.Domain.Entities.BuyerAggregate;
using AnyProduct.Orders.Domain.Entities.OrderAggregate;

namespace AnyProduct.Orders.Application.Services;

public interface IPaymentGatewayService
{
    Task<string> CreateRequest(Order order, PaymentMethod paymentMethod, Uri? webhookUrl);
    Task<bool> Execute(string requestId);
    Task<bool> Refund(string requestId, Uri? webhookUrl);
}
