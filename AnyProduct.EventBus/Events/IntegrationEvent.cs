using System.Text.Json.Serialization;

namespace AnyProduct.EventBus.Events;
public class IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
        Type = this.GetType().Name;
        PartitionKey = null;
    }

    public IntegrationEvent(string partitionKey) :
        this()
    {
        PartitionKey = partitionKey;
    }

    public string Type { get; set; }

    [JsonInclude]
    public Guid Id { get; set; }

    [JsonIgnore]
    public string? PartitionKey { get; set; }

    [JsonInclude]
    public DateTime CreationDate { get; set; }
}
