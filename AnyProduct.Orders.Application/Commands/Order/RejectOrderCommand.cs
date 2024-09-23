using AnyProduct.Orders.Application.IntegrationEvents.Models;
using AnyProduct.Orders.Domain.Entities.OrderAggregate;
using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Application.Commands.Order;

public class RejectOrderCommand : IRequest<Unit>
{
    public required Guid OrderId { get; set; }

    public required ICollection<Guid> RejectedProducts { get; init; }
    public required ICollection<OrderStockDetailedItem> OrderStockDetailedItems { get; init; }
}

public class RejectOrderCommandHandler : IRequestHandler<RejectOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;

    public RejectOrderCommandHandler(IDateTimeProvider dateTimeProvider, IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle([NotNull] RejectOrderCommand request, CancellationToken cancellationToken)
    {

        var order = await _orderRepository.FindByIdAsync(request.OrderId);

        foreach (var item in request.OrderStockDetailedItems)
        {
            order!.UpdateStockDetailsForItem(item.ProductId, item.ProductName, item.UniPrice, item.ImageUrl);
        }

        order!.SetCancelledStatusWhenStockIsRejected(request.RejectedProducts);

        _orderRepository.Update(order);

        return Unit.Value;
    }

}
