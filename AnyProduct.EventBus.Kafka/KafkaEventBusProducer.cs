using AnyProduct.EventBus.Abstractions;
using AnyProduct.EventBus.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Net.Sockets;
using System.Text.Json;

namespace AnyProduct.EventBus.Kafka;

public sealed class KafkaEventBusProducer(
    ILogger<KafkaEventBusProducer> logger,
    IServiceProvider serviceProvider,
    IProducer<string, string> producer,
    IOptions<EventBusOptions> options,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions) : IEventBus, IDisposable
{
    private readonly EventBusSubscriptionInfo _subscriptionInfo = subscriptionOptions.Value;
    private readonly ResiliencePipeline<DeliveryResult<string, string>> _pipeline = CreateResiliencePipeline(options.Value.RetryCount);

    public async Task PublishAsync(IntegrationEvent @event)
    {
        var eventName = @event.GetType().Name;
        var body = SerializeMessage(@event);

        var message = new Message<string, string> { Key = @event.PartitionKey, Value = body };

        await _pipeline.ExecuteAsync(async (CancellationToken) =>
        {
            var result = await producer.ProduceAsync(@event.Type, message, CancellationToken);

            if (result.Status == PersistenceStatus.Persisted)
            {
                logger.LogWarning("Problem publishing event to Kafka: {EventId}", @event.Id);
            }
            else
            {
                logger.LogInformation("Published event to Kafka: {EventId} ({EventName})", @event.Id, eventName);
            }

            return result;
        });

    }

    public void Dispose()
    {
        producer?.Dispose();
    }

    private string SerializeMessage(IntegrationEvent @event)
    {
        return JsonSerializer.Serialize(@event, @event.GetType());
    }

    private static ResiliencePipeline<DeliveryResult<string, string>> CreateResiliencePipeline(int retryCount)
    {
        var retryOptions = new RetryStrategyOptions<DeliveryResult<string, string>>
        {
            ShouldHandle = new PredicateBuilder<DeliveryResult<string, string>>()
            .Handle<KafkaRetriableException>()
            .Handle<SocketException>()
            .HandleResult(result => result.Status != PersistenceStatus.Persisted),
            MaxRetryAttempts = retryCount,
            DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder<DeliveryResult<string, string>>()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }
}
