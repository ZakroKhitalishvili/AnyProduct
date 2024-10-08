﻿
using AnyProduct.Orders.Application.Dtos.Basket;
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Repositories;
using MediatR;

namespace AnyProduct.Orders.Application.Queries;

public class GetBasketQuery : IRequest<ICollection<BasketItemDto>>
{
}

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, ICollection<BasketItemDto>>
{
    private readonly IBasketRepository _basketRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GetBasketQueryHandler(IBasketRepository basketRepository, ICurrentUserProvider currentUserProvider)
    {
        _basketRepository = basketRepository;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<ICollection<BasketItemDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {

        var basket = await _basketRepository.FindByCustomerIdAsync(_currentUserProvider.UserId);

        return basket?.Items.Select(x => new BasketItemDto
        {
            ProductId = x.ProductId,
            Units = x.Units,
        }).ToArray() ?? [];
    }
}
