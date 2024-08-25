
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Entities.Basket;
using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using MediatR;

namespace AnyProduct.Orders.Application.Commands.Basket;

public class UpdateBasketCommand : IRequest<Unit>
{
    public ICollection<BasketItemDto> BasketItems { get; set; }
}

public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, Unit>
{
    private readonly IBasketRepository _basketRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateBasketCommandHandler(IBasketRepository basketRepository, ICurrentUserProvider currentUserProvider, IDateTimeProvider dateTimeProvider)
    {
        _basketRepository = basketRepository;
        _currentUserProvider = currentUserProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Unit> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.FindByCustomerIdAsync(_currentUserProvider.UserId);

        try
        {
            if (basket == null)
            {
                basket = new CustomerBasket(_currentUserProvider.UserId);

                foreach (var item in request.BasketItems)
                {
                    basket.AddOrUpdateItem(item.ProductId, item.Units, _dateTimeProvider.Now);
                }

                _basketRepository.Add(basket);
            }
            else
            {
                foreach (var item in request.BasketItems)
                {
                    basket.AddOrUpdateItem(item.ProductId, item.Units, _dateTimeProvider.Now);
                }

                _basketRepository.Update(basket);
            }
        }
        catch (Exception ex)
        {
        }

        return Unit.Value;
    }
}
