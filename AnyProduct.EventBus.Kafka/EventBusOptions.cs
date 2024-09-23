namespace AnyProduct.EventBus.Kafka;

public class EventBusOptions
{
    public required string BootstrapServers { get; set; }

    public required string ClientId { get; set; } 

    public required string GroupId { get; set; }

    public int RetryCount { get; set; } = 10;
}
