
using AnyProduct.Orders.Application.IntegrationEvents;
using AnyProduct.Orders.Application.IntegrationEvents.Models;
using AnyProduct.Orders.Domain.Entities.Buyer;
using AnyProduct.Orders.Domain.Events;
using AnyProduct.Orders.Domain.Repositories;
using MediatR;

namespace AnyProduct.Orders.Application.DomainEventHandlers;

public class OrderStartedDomainEventHandler : INotificationHandler<DomainEventNotification<OrderStartedDomainEvent>>
{
    private readonly IIntegrationEventService _integrationEventService;
    private readonly IBasketRepository _basketRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderStartedDomainEventHandler(IIntegrationEventService integrationEventService, IBasketRepository basketRepository, IBuyerRepository buyerRepository, IOrderRepository orderRepository)
    {
        _integrationEventService = integrationEventService;
        _basketRepository = basketRepository;
        _buyerRepository = buyerRepository;
        _orderRepository = orderRepository;
    }

    public async Task Handle(DomainEventNotification<OrderStartedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.FindByCustomerIdAsync(notification.DomainEvent.UserId);
        var buyer = await _buyerRepository.FindByCustomerIdAsync(notification.DomainEvent.UserId);

        if (basket is { })
        {
            basket.ClearItems();
            _basketRepository.Update(basket);
        }

        bool buyerExisted = buyer is { };

        if (!buyerExisted)
        {
            buyer = new Buyer(notification.DomainEvent.UserId, notification.DomainEvent.UserName);

        }

        var paymentMethod = buyer!.VerifyOrAddPaymentMethod(notification.DomainEvent.CardType,
                                        $"Payment Method on {DateTime.UtcNow}",
                                        notification.DomainEvent.CardNumber,
                                        notification.DomainEvent.CardSecurityNumber,
                                        notification.DomainEvent.CardHolderName,
                                        notification.DomainEvent.CardExpiration,
                                        notification.DomainEvent.Order.AggregateId);

        notification.DomainEvent.Order.SetPaymentMethod(paymentMethod.Id.ToString());

        _orderRepository.Update(notification.DomainEvent.Order);

        if (!buyerExisted)
        {
            _buyerRepository.Add(buyer);
        }
        else
        {
            _buyerRepository.Update(buyer);
        }


        await _integrationEventService.AddEventAsync(
            new OrderStartedIntegrationEvent(
                notification.DomainEvent.UserId,
            notification.DomainEvent.Order.AggregateId,
            notification.DomainEvent.Order.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units)).ToArray()
            ));
    }
}
