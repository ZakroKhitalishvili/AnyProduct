namespace AnyProduct.Orders.Application.Behaviours;

using AnyProduct.Orders.Application.IntegrationEvents;
using AnyProduct.Orders.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly IUnitOfWork _transactionService;
    private readonly IIntegrationEventService _integrationEventService;

    public TransactionBehavior(
        IIntegrationEventService orderingIntegrationEventService,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger,
        IUnitOfWork transactionService)
    {
        _integrationEventService = orderingIntegrationEventService ?? throw new ArgumentException(nameof(orderingIntegrationEventService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        _transactionService = transactionService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);

        if (!typeof(TRequest).Name.EndsWith("Command"))
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
        catch (Exception ex)
        {
            _transactionService.Rollback();
            throw;
        }
    }
}
