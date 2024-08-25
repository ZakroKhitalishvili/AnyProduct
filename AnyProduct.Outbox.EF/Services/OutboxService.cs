using AnyProduct.EventBus.Events;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AnyProduct.OutBox.EF.Services;

public class EFOutboxService<TContext> : IOutboxService, IDisposable
    where TContext : DbContext
{
    private volatile bool _disposedValue;
    private readonly TContext _context;
    private readonly Type[] _eventTypes;

    public EFOutboxService(TContext context, Assembly intergationEventAssembly)
    {
        _context = context;
        _eventTypes = Assembly.Load(intergationEventAssembly.FullName)
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(IntegrationEvent)))
            .ToArray();

    }

    public async Task<IEnumerable<IntegrationEventLogEntry>> GetPendingEventsAsync(string transactionId)
    {
        Guid transactionGuidId = Guid.Parse(transactionId);

        var result = await _context.Set<IntegrationEventLogEntry>()
            .Where(e => e.TransactionId == transactionGuidId && e.State == EventStateEnum.NotPublished)
            .ToListAsync();

        if (result.Count != 0)
        {
            return result.OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.FirstOrDefault(t => t.Name == e.EventTypeShortName)));
        }

        return [];
    }

    public Task SaveEventAsync(IntegrationEvent @event)
    {
        if (
        _context.Database.CurrentTransaction == null) throw new Exception("Saving requires an active transaction around EF DbContext");

        var eventLogEntry = new IntegrationEventLogEntry(@event,
        _context.Database.CurrentTransaction.TransactionId);

        _context.Set<IntegrationEventLogEntry>().Add(eventLogEntry);

        return _context.SaveChangesAsync();
    }

    public Task MarkEventAsPublishedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.Published);
    }

    public Task MarkEventAsInProgressAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.InProgress);
    }

    public Task MarkEventAsFailedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
    }

    private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
    {
        var eventLogEntry = _context.Set<IntegrationEventLogEntry>().Single(ie => ie.EventId == eventId);
        eventLogEntry.State = status;

        if (status == EventStateEnum.InProgress)
            eventLogEntry.TimesSent++;

        return _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                //_context.Dispose();
            }


            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
