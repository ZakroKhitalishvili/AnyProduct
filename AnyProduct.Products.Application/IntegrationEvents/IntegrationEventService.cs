using AnyProduct.EventBus.Abstractions;
using AnyProduct.EventBus.Events;
using AnyProduct.OutBox.EF.Services;
using AnyProduct.Products.Application.IntegrationEvents;

namespace eShop.Ordering.API.Application.IntegrationEvents;

public class IntegrationEventService(IEventBus eventBus,
    IOutboxService outboxService) : IIntegrationEventService
{
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    private readonly IOutboxService _outboxService = outboxService ?? throw new ArgumentNullException(nameof(outboxService));

    public async Task PublishEventsAsync(string transactionId)
    {
        var pendingLogEvents = await _outboxService.GetPendingEventsAsync(transactionId);

        foreach (var logEvt in pendingLogEvents)
        {
            try
            {
                await _outboxService.MarkEventAsInProgressAsync(logEvt.EventId);
                await _eventBus.PublishAsync(logEvt.IntegrationEvent);
                await _outboxService.MarkEventAsPublishedAsync(logEvt.EventId);
            }
            catch (Exception ex)
            {
                await _outboxService.MarkEventAsFailedAsync(logEvt.EventId);
            }
        }
    }

    public async Task AddEventAsync(IntegrationEvent evt)
    {
        await _outboxService.SaveEventAsync(evt);
    }
}
