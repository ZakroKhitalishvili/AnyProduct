using AnyProduct.EventBus.Events;

namespace AnyProduct.Inbox.EF.Services;

public interface IInboxService
{
    Task<bool> TryProcessAsync(IntegrationEvent @event, string handler);
    Task<bool> FailAsync(Guid eventId, string handler);
    Task<bool> SucceedAsync(Guid eventId, string handler);
    Task<bool> ResetAsync(Guid eventId, string handler);
    Task<ICollection<InboxEventLogEntry>> GetFailedEvents(string handler);
}
