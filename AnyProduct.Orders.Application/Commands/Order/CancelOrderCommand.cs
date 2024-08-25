using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using MediatR;

namespace AnyProduct.Orders.Application.Commands.Order;

public class CancelOrderCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
}

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Unit>
{

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _accountRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CancelOrderCommandHandler(IDateTimeProvider dateTimeProvider, IOrderRepository orderRepository, IPaymentRepository accountRepository, ICurrentUserProvider currentUserProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
        _accountRepository = accountRepository;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.OrderId);

        if (_currentUserProvider.UserId != order.BuyerId)
        {
            throw new Exception("Order does not exist!");
        }

        order.SetCancelledStatus();

        _orderRepository.Update(order);

        return Unit.Value;
    }

}
