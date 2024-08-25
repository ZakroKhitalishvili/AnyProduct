
using AnyProduct.Orders.Domain.Events;
using MediatR;
using AnyProduct.Orders.Application.Commands.Order;

namespace AnyProduct.Orders.Application.DomainEventHandlers;

public class StockConfirmedDomainEventHandler : INotificationHandler<DomainEventNotification<StockConfirmedDomainEvent>>
{
    public readonly IMediator _mediator;

    public StockConfirmedDomainEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(DomainEventNotification<StockConfirmedDomainEvent> notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ExecuteOrderCommand() { OrderId = notification.DomainEvent.OrderId });
    }
}
