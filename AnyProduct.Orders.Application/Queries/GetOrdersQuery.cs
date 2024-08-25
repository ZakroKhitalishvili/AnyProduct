

using AnyProduct.Orders.Application.Dtos;
using AnyProduct.Orders.Domain.Repositories;
using MediatR;

namespace AnyProduct.Orders.Application.Queries;

public class GetOrderQuery : IRequest<PagedListDto<OrderDto>>
{
    public int? Page { get; set; }

    public int? PageSize { get; set; }
}

public class GetOrdersQueryHandler : IRequestHandler<GetOrderQuery, PagedListDto<OrderDto>>
{
    public readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedListDto<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        request.Page ??= 1;
        request.PageSize ??= 10;

        var orders = _orderRepository.GetList(out int totalSize, null, request.Page.Value, request.PageSize.Value);

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


        return result;
    }
}
