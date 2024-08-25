using AnyProduct.Orders.Application.IntegrationEvents.Models;
using AnyProduct.Orders.Domain.Entities.Order;
using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AnyProduct.Orders.Application.Commands.Order;

public class RejectOrderCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }

    public ICollection<Guid> RejectedProducts { get; set; }
    public ICollection<OrderStockDetailedItem> OrderStockDetailedItems { get; internal set; }
}

public class RejectOrderCommandHandler : IRequestHandler<RejectOrderCommand, Unit>
{

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderRepository _orderRepository;

    public RejectOrderCommandHandler(IDateTimeProvider dateTimeProvider, IOrderRepository orderRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(RejectOrderCommand request, CancellationToken cancellationToken)
    {

        var order = await _orderRepository.FindByIdAsync(request.OrderId);

        foreach (var item in request.OrderStockDetailedItems)
        {
            order.UpdateStockDetailsForItem(item.ProductId, item.ProductName, item.UniPrice, item.ImageUrl);
        }

        order.SetCancelledStatusWhenStockIsRejected(request.RejectedProducts);

        _orderRepository.Update(order);

        return Unit.Value;
    }

}
