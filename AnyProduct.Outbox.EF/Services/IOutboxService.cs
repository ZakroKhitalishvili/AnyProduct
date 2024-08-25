using AnyProduct.EventBus.Events;

namespace AnyProduct.OutBox.EF.Services;

public interface IOutboxService
{
    Task<IEnumerable<IntegrationEventLogEntry>> GetPendingEventsAsync(string transactionId);
    Task SaveEventAsync(IntegrationEvent @event);
    Task MarkEventAsPublishedAsync(Guid eventId);
    Task MarkEventAsInProgressAsync(Guid eventId);
    Task MarkEventAsFailedAsync(Guid eventId);
}
