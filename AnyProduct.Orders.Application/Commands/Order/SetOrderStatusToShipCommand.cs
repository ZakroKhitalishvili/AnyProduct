using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using MediatR;

namespace AnyProduct.Orders.Application.Commands.Order;

public class SetOrderStatusToShipCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
}

public class SetOrderStatusToShipCommandHandler : IRequestHandler<SetOrderStatusToShipCommand, Unit>
{

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderRepository _orderRepository;

    public SetOrderStatusToShipCommandHandler(IDateTimeProvider dateTimeProvider, IOrderRepository orderRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(SetOrderStatusToShipCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.OrderId);

        order.SetShippedStatus();

        _orderRepository.Update(order);

        return Unit.Value;
    }

}
