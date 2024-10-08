﻿using AnyProduct.EventBus.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace AnyProduct.OutBox.EF;

public class IntegrationEventLogEntry
{
    private static readonly JsonSerializerOptions s_indentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions s_caseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };
   
    private IntegrationEventLogEntry() { }

    public IntegrationEventLogEntry([NotNull] IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.CreationDate;
        EventTypeName = @event.GetType().FullName!;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), s_indentedOptions);
        State = EventState.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId;
    }

    public Guid EventId { get; private set; }
    [Required]
    public string EventTypeName { get; private set; }
    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.').Last();
    [NotMapped]
    public IntegrationEvent? IntegrationEvent { get; private set; }
    public EventState State { get; set; }
    public int TimesSent { get; set; }
    public DateTime CreationTime { get; private set; }
    [Required]
    public string Content { get; private set; }
    public Guid TransactionId { get; private set; }

    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, s_caseInsensitiveOptions) as IntegrationEvent;
        return this;
    }
}
