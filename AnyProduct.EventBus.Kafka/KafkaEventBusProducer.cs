using AnyProduct.EventBus.Abstractions;
using AnyProduct.EventBus.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly;
using Polly.Retry;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace AnyProduct.EventBus.Kafka;

public sealed class KafkaEventBusProducer(
    ILogger<KafkaEventBusProducer> logger,
    IServiceProvider serviceProvider,
    IProducer<string, string> producer,
    IOptions<EventBusOptions> options,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions,
    KafkaTelemetry kafkaTelemetry) : IEventBus, IDisposable
{
    private readonly ActivitySource _activitySource = kafkaTelemetry.ActivitySource;
    private readonly TextMapPropagator _propagator = kafkaTelemetry.Propagator;

    private readonly EventBusSubscriptionInfo _subscriptionInfo = subscriptionOptions.Value;
    private readonly ResiliencePipeline<DeliveryResult<string, string>> _pipeline = CreateResiliencePipeline(options.Value.RetryCount);

    public async Task PublishAsync(IntegrationEvent @event)
    {
        var eventName = @event.GetType().Name;
        var body = SerializeMessage(@event);

        var message = new Message<string, string> { Key = @event.PartitionKey, Value = body };

        var activityName = $"{eventName} publish";

        await _pipeline.ExecuteAsync(async (CancellationToken) =>
        {

            using var activity = _activitySource.StartActivity(activityName, ActivityKind.Producer);

            // Depending on Sampling (and whether a listener is registered or not), the activity above may not be created.
            // If it is created, then propagate its context. If it is not created, the propagate the Current context, if any.

            ActivityContext contextToInject = default;

            if (activity != null)
            {
                contextToInject = activity.Context;
            }
            else if (Activity.Current != null)
            {
                contextToInject = Activity.Current.Context;
            }

            _propagator.Inject(new PropagationContext(contextToInject, Baggage.Current), message, InjectTraceContextIntoMessage);

            activity.SetActivityContext(@event.Type, "publish");

            try
            {
                var result = await producer.ProduceAsync(@event.Type, message, CancellationToken);

                if (result.Status != PersistenceStatus.Persisted)
                {
                    logger.LogWarning("Problem publishing event to Kafka: {EventId}", @event.Id);
                }
                else
                {
                    logger.LogInformation("Published event to Kafka: {EventId} ({EventName})", @event.Id, eventName);
                }

                return result;
            }
            catch (Exception ex)
            {
                activity.SetExceptionTags(ex);

                throw;
            }

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

    private void InjectTraceContextIntoMessage(Message<string, string> message, string key, string value)
    {
        var header = new Headers();
        header.Add(key, Encoding.UTF8.GetBytes(value));
        message.Headers = header;
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
