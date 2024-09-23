
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Entities.BuyerAggregate;
using AnyProduct.Orders.Domain.Entities.OrderAggregate;

namespace AnyProduct.Orders.Infrastructure.Services;

public class PaymentGatewayService : IPaymentGatewayService
{
    public Task<string> CreateRequest(Order order, PaymentMethod paymentMethod, Uri? webhookUrl)
    {
        Task.Delay(1000);

        return Task.FromResult(Guid.NewGuid().ToString());
    }

    public Task<bool> Execute(string requestId)
    {
        Task.Delay(5000);

        return Task.FromResult(true);
    }

    public Task<bool> Refund(string requestId, Uri? webhookUrl)
    {
        Task.Delay(5000);

        return Task.FromResult(true);
    }
}
