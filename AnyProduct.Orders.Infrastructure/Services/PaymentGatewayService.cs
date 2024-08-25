
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Entities.Buyer;
using AnyProduct.Orders.Domain.Entities.Order;

namespace AnyProduct.Orders.Infrastructure.Services;

public class PaymentGatewayService : IPaymentGatewayService
{
    public Task<string> CreateRequest(Order order, PaymentMethod paymentMethod, string webhookUrl)
    {
        Task.Delay(1000);

        return Task.FromResult(Guid.NewGuid().ToString());
    }

    public Task<bool> Execute(string requestId)
    {
        Task.Delay(5000);

        return Task.FromResult(true);
    }

    public Task<bool> Refund(string requestId, string webhookUrl)
    {
        Task.Delay(5000);

        return Task.FromResult(true);
    }
}
