
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using MediatR;

namespace AnyProduct.Orders.Application.Commands.Basket;

public class DeleteBasketCommand : IRequest<Unit>
{
}

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Unit>
{
    private readonly IBasketRepository _basketRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository, ICurrentUserProvider currentUserProvider, IDateTimeProvider dateTimeProvider)
    {
        _basketRepository = basketRepository;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<Unit> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.FindByCustomerIdAsync(_currentUserProvider.UserId);

        if (basket is { })
        {
            basket.ClearItems();

            _basketRepository.Update(basket);
        }

        return Unit.Value;
    }
}
