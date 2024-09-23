using AnyProduct.Products.Application.IntegrationEvents;
using AnyProduct.Products.Application.IntegrationEvents.Models;
using AnyProduct.Products.Domain.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Commands;

public class RemoveReservationCommand : IRequest<Unit>
{
    public required Guid OrderId { get; set; }
    public required IReadOnlyCollection<OrderStockItem> OrderStockItems { get; set; }

}

public class RemoveReservationCommandHandler : IRequestHandler<RemoveReservationCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IIntegrationEventService _integrationEventService;

    public RemoveReservationCommandHandler(IProductRepository productRepository, IIntegrationEventService integrationEventService)
    {
        _productRepository = productRepository;
        _integrationEventService = integrationEventService;
    }

    public async Task<Unit> Handle([NotNull] RemoveReservationCommand request, CancellationToken cancellationToken)
    {

        foreach (var stockItem in request.OrderStockItems)
        {
            var product = _productRepository.FindById(stockItem.ProductId);

            if (product is null)
            {
                throw new InvalidOperationException("Product not found");
            }
            else
            {
                product.RemoveReservation(stockItem.Units);
                _productRepository.Update(product);
            }
        }

        // perform other operations relevant to a shipment

        await _integrationEventService.AddEventAsync(new OrderShippedIntergationEvent(request.OrderId));

        return Unit.Value;
    }
}
