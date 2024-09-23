namespace AnyProduct.Products.Application.Behaviours;

using AnyProduct.Products.Application.IntegrationEvents;
using AnyProduct.Products.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _transactionService;
    private readonly IIntegrationEventService _integrationEventService;

    public TransactionBehavior(
        IIntegrationEventService orderingIntegrationEventService,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger,
        IUnitOfWork transactionService)
    {
        _integrationEventService = orderingIntegrationEventService;
        _transactionService = transactionService;
    }

    public async Task<TResponse> Handle(TRequest request, [NotNull] RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);

        if (!typeof(TRequest).Name.EndsWith("Command", StringComparison.OrdinalIgnoreCase))
        {
            return await next();
        }

        try
        {
            if (_transactionService.IsActive)
            {
                return await next();
            }

            string transactionId = await _transactionService.BeginAsync();

            response = await next();

            await _transactionService.CommitAsync();

            await _integrationEventService.PublishEventsAsync(transactionId);

            return response;
        }
        catch (Exception)
        {
            _transactionService.Rollback();
            throw;
        }
    }
}
