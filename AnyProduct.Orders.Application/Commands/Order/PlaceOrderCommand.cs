using AnyProduct.Orders.Application.Dtos;
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Entities.Order;
using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Application.Commands.Order;

public class PlaceOrderCommand : IRequest<PlacedOrderDto>
{

    [Required]
    public string UserName { get; set; }

    [Required]
    public ShippingAddressDto ShippingAddress { get; set; }

    [Required]
    public CardDataDto CardData { get; set; }

    [Required]
    public ICollection<RequestedOrderItemDto> OrderItems { get; set; }
}

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlacedOrderDto>
{

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public PlaceOrderCommandHandler(IDateTimeProvider dateTimeProvider, IOrderRepository orderRepository, ICurrentUserProvider currentUserProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
        _currentUserProvider = currentUserProvider;
    }

    public Task<PlacedOrderDto> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {

        var address = new AddressValueObject(request.ShippingAddress.Street, request.ShippingAddress.City, request.ShippingAddress.State, request.ShippingAddress.Country, request.ShippingAddress.ZipCode);
        var order = new Domain.Entities.Order.Order(_currentUserProvider.UserId, request.UserName, address, request.CardData.CardType, request.CardData.CardNumber, request.CardData.CardSecurityNumber, request.CardData.CardHolderName, request.CardData.CardExpiration);

        foreach (var item in request.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.Units);
        }

        _orderRepository.Add(order);

        return Task.FromResult(PlacedOrderDto.From(order));
    }

}
