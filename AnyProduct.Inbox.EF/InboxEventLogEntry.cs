using AnyProduct.EventBus.Events;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Query;

namespace AnyProduct.Inbox.EF;

public class InboxEventLogEntry
{
    private static readonly JsonSerializerOptions s_indentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions s_caseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    private InboxEventLogEntry() { }
    public InboxEventLogEntry([NotNull] IntegrationEvent @event, string handler)
    {
        EventId = @event.Id;
        CreationTime = DateTime.UtcNow;
        EventTypeName = @event.GetType().FullName!;
        State = InboxEventState.NotConsumed;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), s_indentedOptions);
        TimesConsumed = 0;
        Handler = handler;
    }
    public Guid EventId { get; private set; }
    public string Handler { get; private set; }
    [Required]
    public string EventTypeName { get; private set; }
    [NotMapped]
    public string? EventTypeShortName => EventTypeName.Split('.').Last();
    public InboxEventState State { get; set; }
    public int TimesConsumed { get; set; }
    public DateTime CreationTime { get; private set; }
    [NotMapped]
    public IntegrationEvent? IntegrationEvent { get; private set; }
    [Required]
    public string Content { get; private set; }
    public uint Version { get; set; } // concurrency control

    public InboxEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, s_caseInsensitiveOptions) as IntegrationEvent;
        return this;
    }
}
