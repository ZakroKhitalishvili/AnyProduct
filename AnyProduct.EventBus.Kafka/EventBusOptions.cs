namespace AnyProduct.EventBus.Kafka;

public class EventBusOptions
{
    public string BootstrapServers { get; set; }

    public string ClientId { get; set; } 

    public string GroupId { get; set; }

    public int RetryCount { get; set; } = 10;
}
