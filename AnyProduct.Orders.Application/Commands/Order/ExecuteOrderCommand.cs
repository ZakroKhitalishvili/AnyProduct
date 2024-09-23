using AnyProduct.Orders.Application.IntegrationEvents.Models;
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Entities.PaymentAggregate;
using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Application.Commands.Order;

public class ExecuteOrderCommand : IRequest<Unit>
{
    public required Guid OrderId { get; set; }
    public required ICollection<OrderStockDetailedItem> OrderStockDetailedItems { get; init; }
}

public class ExecuteOrderCommandHandler : IRequestHandler<ExecuteOrderCommand, Unit>
{

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IPaymentGatewayService _paymentGatewayService;

    public ExecuteOrderCommandHandler(IDateTimeProvider dateTimeProvider, IOrderRepository orderRepository, IPaymentRepository accountRepository, ICurrentUserProvider currentUserProvider, IPaymentGatewayService paymentGatewayService, IBuyerRepository buyerRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
        _paymentRepository = accountRepository;
        _paymentGatewayService = paymentGatewayService;
        _buyerRepository = buyerRepository;
    }

    public async Task<Unit> Handle([NotNull] ExecuteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.OrderId);
        var buyer = await _buyerRepository.FindByCustomerIdAsync(order!.BuyerId!);

        foreach (var item in request.OrderStockDetailedItems)
        {
            order.UpdateStockDetailsForItem(item.ProductId, item.ProductName, item.UniPrice, item.ImageUrl);
        }

        string? requestId = null;

        try
        {

            var paymentMethod = buyer!.PaymentMethods.Single(x => x.Id.ToString() == order.PaymentMethodId);

            requestId = await _paymentGatewayService.CreateRequest(order, paymentMethod, null);

            var payment = new Payment(_dateTimeProvider.Now, order.GetTotal(), order.AggregateId, order.BuyerId!, requestId, paymentMethod.GetBriefDescription());

            order.SetPaidStatus(payment.AggregateId);

            _paymentRepository.Add(payment);

            _orderRepository.Update(order);

            await _paymentGatewayService.Execute(requestId);

        }
        catch
        {
            if (requestId is not null)
            {
                await _paymentGatewayService.Refund(requestId, null);
            }

            order.SetCancelledStatusWhenPaymentRejected();

            _orderRepository.Update(order);
        }


        return Unit.Value;
    }

}
