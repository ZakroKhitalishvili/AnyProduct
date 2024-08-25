using AnyProduct.EventBus.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnyProduct.Inbox.EF.Services;

public class EFInboxService<TContext> : IInboxService
    where TContext : DbContext
{
    private volatile bool _disposedValue;
    private readonly TContext _context;
    private readonly InboxOptions _options;
    public EFInboxService(TContext context, IOptions<InboxOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    public async Task<bool> TryProcessAsync(IntegrationEvent @event, string handler)
    {
        var eventLogEntry = _context.Set<InboxEventLogEntry>().SingleOrDefault(x => x.EventId == @event.Id && x.Handler == handler);

        if (eventLogEntry is null)
        {
            eventLogEntry = new InboxEventLogEntry(@event, handler);
            eventLogEntry.State = InboxEventStateEnum.InProgress;
            eventLogEntry.TimesConsumed = 1;

            _context.Add(eventLogEntry);
            await _context.SaveChangesAsync();
            return true;
        }

        if (eventLogEntry.State == InboxEventStateEnum.NotConsumed)
        {
            if (eventLogEntry.TimesConsumed < _options.RetryCount)
            {
                eventLogEntry.State = InboxEventStateEnum.InProgress;
                eventLogEntry.TimesConsumed++;

                await _context.SaveChangesAsync();

                return true;
            }

        }

        return false;
    }

    public async Task<bool> SucceedAsync(Guid eventId, string handler)
    {
        var eventLogEntry = _context.Set<InboxEventLogEntry>().Single(x => x.EventId == eventId && x.Handler == handler);

        if (eventLogEntry.State == InboxEventStateEnum.InProgress)
        {

            eventLogEntry.State = InboxEventStateEnum.Consumed;

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> FailAsync(Guid eventId, string handler)
    {
        var eventLogEntry = _context.Set<InboxEventLogEntry>().Single(x => x.EventId == eventId && x.Handler == handler);

        if (eventLogEntry.State == InboxEventStateEnum.InProgress)
        {
            eventLogEntry.State = InboxEventStateEnum.NotConsumed;

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> ResetAsync(Guid eventId, string handler)
    {
        var eventLogEntry = _context.Set<InboxEventLogEntry>().Single(x => x.EventId == eventId && x.Handler == handler);

        if (eventLogEntry.State == InboxEventStateEnum.ConsumeFailed)
        {
            eventLogEntry.State = InboxEventStateEnum.InProgress;
            eventLogEntry.TimesConsumed = 0;

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<ICollection<InboxEventLogEntry>> GetFailedEvents(string handler)
    {
        return await _context.Set<InboxEventLogEntry>()
            .Where(x => x.Handler == handler && x.State == InboxEventStateEnum.ConsumeFailed)
            .ToListAsync();

    }
}
