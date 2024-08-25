﻿using AnyProduct.Products.Application.IntegrationEvents;
using AnyProduct.Products.Application.IntegrationEvents.Models;
using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using MediatR;

namespace AnyProduct.Products.Application.Commands;

public class RemoveReservationCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
    public ICollection<OrderStockItem> OrderStockItems { get; set; }

}

public class RemoveReservationCommandHandler : IRequestHandler<RemoveReservationCommand, Unit>
{
    public readonly IProductRepository _productRepository;
    public readonly IIntegrationEventService _integrationEventService;

    public RemoveReservationCommandHandler(IProductRepository productRepository, IIntegrationEventService integrationEventService)
    {
        _productRepository = productRepository;
        _integrationEventService = integrationEventService;
    }

    public async Task<Unit> Handle(RemoveReservationCommand request, CancellationToken cancellationToken)
    {
        var rejectedProducts = new List<Guid>();

        foreach (var stockItem in request.OrderStockItems)
        {
            var product = _productRepository.FindById(stockItem.ProductId);

            if (product is null)
            {
                throw new Exception("Product not found");
            }
            else
            {
                product.RemoveReservation(stockItem.Units);
                _productRepository.Update(product);
            }
        }

        // performa other operations relevant to shipment

        await _integrationEventService.AddEventAsync(new OrderShippedIntergationEvent(request.OrderId));

        return Unit.Value;
    }
}
