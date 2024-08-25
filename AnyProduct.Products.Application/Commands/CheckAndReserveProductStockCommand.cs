using AnyProduct.Products.Application.IntegrationEvents;
using AnyProduct.Products.Application.IntegrationEvents.Models;
using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AnyProduct.Products.Application.Commands;

public class CheckAndReserveProductStockCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
    public ICollection<OrderStockItem> OrderStockItems { get; set; }

}

public class CheckAndReserveProductStockCommandHandler : IRequestHandler<CheckAndReserveProductStockCommand, Unit>
{
    public readonly IProductRepository _productRepository;
    public readonly IIntegrationEventService _integrationEventService;
    public readonly IConfiguration _configuration;

    public CheckAndReserveProductStockCommandHandler(IProductRepository productRepository, IIntegrationEventService integrationEventService, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _integrationEventService = integrationEventService;
        _configuration = configuration;
    }

    public async Task<Unit> Handle(CheckAndReserveProductStockCommand request, CancellationToken cancellationToken)
    {
        var rejectedProducts = new List<Guid>();

        var products = new List<(OrderStockItem, Product)>();

        var stockDetailedItems = new List<OrderStockDetailedItem>();

        foreach (var stockItem in request.OrderStockItems)
        {
            var product = _productRepository.FindById(stockItem.ProductId);

            if (product is null || product.Amount < stockItem.Units)
            {
                rejectedProducts.Add(stockItem.ProductId);
            }
            else
            {
                products.Add((stockItem, product));
                stockDetailedItems.Add(new OrderStockDetailedItem(
                    product.AggregateId,
                    product.Name,
                    $"{_configuration["FileUpload:BaseUrl"]}/{product.Image}",
                   product.Price,
                   product.Amount
                    ));

            }
        }


        if (rejectedProducts.Any())
        {
            await _integrationEventService.AddEventAsync(new OrderStockRejectedIntergationEvent(request.OrderId, rejectedProducts, stockDetailedItems));
        }
        else
        {
            foreach (var (stockItem, product) in products)
            {
                product.Reserve(stockItem.Units);
                _productRepository.Update(product);
            }

            await _integrationEventService.AddEventAsync(new OrderStockConfirmedIntergationEvent(request.OrderId, stockDetailedItems));
        }

        return Unit.Value;
    }
}
