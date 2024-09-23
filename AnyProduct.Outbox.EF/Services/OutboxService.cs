using AnyProduct.EventBus.Events;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AnyProduct.OutBox.EF.Services;

public class EFOutboxService<TContext> : IOutboxService
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly Type[] _eventTypes;

    public EFOutboxService(TContext context, [NotNull] Assembly intergationEventAssembly)
    {
        _context = context;
        _eventTypes = Assembly.Load(intergationEventAssembly.FullName!)
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(IntegrationEvent)))
            .ToArray();

    }

    public async Task<IEnumerable<IntegrationEventLogEntry>> GetPendingEventsAsync(string transactionId)
    {
        Guid transactionGuidId = Guid.Parse(transactionId);

        var result = await _context.Set<IntegrationEventLogEntry>()
            .Where(e => e.TransactionId == transactionGuidId && e.State == EventState.NotPublished)
            .ToListAsync();

        if (result.Count != 0)
        {
            return result.OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.FirstOrDefault(t => t.Name == e.EventTypeShortName)!));
        }

        return [];
    }

    public Task SaveEventAsync(IntegrationEvent @event)
    {
        if (
        _context.Database.CurrentTransaction == null) throw new InvalidOperationException("Saving requires an active transaction around EF DbContext");

        var eventLogEntry = new IntegrationEventLogEntry(@event,
        _context.Database.CurrentTransaction.TransactionId);

        _context.Set<IntegrationEventLogEntry>().Add(eventLogEntry);

        return _context.SaveChangesAsync();
    }

    public Task MarkEventAsPublishedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.Published);
    }

    public Task MarkEventAsInProgressAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.InProgress);
    }

    public Task MarkEventAsFailedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.PublishedFailed);
    }

    private async Task UpdateEventStatus(Guid eventId, EventState status)
    {
        var eventLogEntry = await _context.Set<IntegrationEventLogEntry>().SingleAsync(ie => ie.EventId == eventId);
        eventLogEntry.State = status;

        if (status == EventState.InProgress)
            eventLogEntry.TimesSent++;

        await _context.SaveChangesAsync();
    }

}
