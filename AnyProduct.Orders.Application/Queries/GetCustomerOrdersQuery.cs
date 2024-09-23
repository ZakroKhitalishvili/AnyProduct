
using AnyProduct.Orders.Application.Dtos;
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Domain.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Application.Queries;

public class GetCustomerOrdersQuery : IRequest<PagedListDto<OrderDto>>
{
    public int? Page { get; set; }

    public int? PageSize { get; set; }
}

public class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, PagedListDto<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GetCustomerOrdersQueryHandler(IOrderRepository orderRepository, ICurrentUserProvider currentUserProvider)
    {
        _orderRepository = orderRepository;
        _currentUserProvider = currentUserProvider;
    }

    public Task<PagedListDto<OrderDto>> Handle([NotNull] GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        request.Page ??= 1;
        request.PageSize ??= 10;

        var orders = _orderRepository.GetList(out int totalSize, _currentUserProvider.UserId, request.Page.Value, request.PageSize.Value);

        var result = new PagedListDto<OrderDto>()
        {
            Items = new List<OrderDto>(),
            Page = request.Page.Value,
            PageSize = request.PageSize.Value,
            Total = totalSize,
        };

        foreach (var order in orders)
        {
            result.Items.Add(OrderDto.From(order));
        }

        return Task.FromResult(result);
    }
}
