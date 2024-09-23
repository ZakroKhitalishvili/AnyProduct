using AnyProduct.Products.Application.IntegrationEvents;
using AnyProduct.Products.Application.IntegrationEvents.Models;
using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Commands;

public class PrepareProductShipmentCommand : IRequest<Unit>
{
    public required Guid OrderId { get; set; }
    public required IReadOnlyCollection<OrderStockItem> OrderStockItems { get; set; }

}

public class PrepareProductShipmentCommandHandler : IRequestHandler<PrepareProductShipmentCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IIntegrationEventService _integrationEventService;

    public PrepareProductShipmentCommandHandler(IProductRepository productRepository, IIntegrationEventService integrationEventService)
    {
        _productRepository = productRepository;
        _integrationEventService = integrationEventService;
    }

    public async Task<Unit> Handle([NotNull] PrepareProductShipmentCommand request, CancellationToken cancellationToken)
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
                product.ShipReservetion(stockItem.Units);
                _productRepository.Update(product);
            }
        }

        // perform other operations relevant to a shipment

        await _integrationEventService.AddEventAsync(new OrderShippedIntergationEvent(request.OrderId));

        return Unit.Value;
    }
}
