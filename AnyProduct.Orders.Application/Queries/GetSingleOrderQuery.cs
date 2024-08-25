

using AnyProduct.Orders.Application.Dtos;
using AnyProduct.Orders.Domain.Repositories;
using MediatR;

namespace AnyProduct.Orders.Application.Queries;

public class GetSingleOrderQuery : IRequest<OrderDto?>
{
    public Guid Id { get; set; }
}

public class GetSingleOrderQueryHandler : IRequestHandler<GetSingleOrderQuery, OrderDto?>
{
    public readonly IOrderRepository _orderRepository;

    public GetSingleOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto?> Handle(GetSingleOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.Id);

        if (order is null) return null;

        return OrderDto.From(order);

    }
}
