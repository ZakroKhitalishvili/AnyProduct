using AnyProduct.EventBus.Abstractions;
using AnyProduct.EventBus.Events;
using AnyProduct.Inbox.EF.Services;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry;
using System.Text;
using System;
using AnyProduct.EventBus.Kafka.Extensions;
namespace AnyProduct.EventBus.Kafka
{

    public sealed class KafkaEventBusConsumer(
        ILogger<KafkaEventBusConsumer> logger,
        IServiceScopeFactory serviceScopeFactory,
        IConsumer<string, string> consumer,
        IAdminClient adminClient,
        IOptions<EventBusSubscriptionInfo> subscriptionOptions,
        KafkaTelemetry kafkaTelemetry) : BackgroundService
    {
#pragma warning disable CA2213 // Disposable fields should be disposed
        private readonly ActivitySource _activitySource = kafkaTelemetry.ActivitySource;
#pragma warning restore CA2213 // Disposable fields should be disposed
        private readonly TextMapPropagator _propagator = kafkaTelemetry.Propagator;

        private readonly EventBusSubscriptionInfo _subscriptionInfo = subscriptionOptions.Value;

        public override void Dispose()
        {
            base.Dispose();
            consumer?.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting Kafka consumer on a background thread");

            try
            {
                var topicScpecifications = _subscriptionInfo.EventTypes.Keys.Select(type => new TopicSpecification()
                {
                    Name = type,
                });

                await adminClient.CreateTopicsAsync(topicScpecifications);
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occurred creating topic: {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }

            consumer.Subscribe(_subscriptionInfo.EventTypes.Keys);

            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<string, string> consumeResult;
                try
                {
                    consumeResult = consumer.Consume(stoppingToken);

                }
                catch (ConsumeException ex)
                {
                    logger.LogWarning(ex, "Kafka consuming timed out");
                    continue;
                }

                var eventName = consumeResult.Topic;
                var message = consumeResult.Message.Value;

                // Extract the PropagationContext of the upstream parent from the message headers.
                var parentContext = _propagator.Extract(default, consumeResult.Message, ExtractTraceContextFromMessage);
                Baggage.Current = parentContext.Baggage;

                // Start an activity with a name following the semantic convention of the OpenTelemetry messaging specification.
                // https://github.com/open-telemetry/semantic-conventions/blob/main/docs/messaging/messaging-spans.md
                var activityName = $"{eventName} receive";

                using var activity = _activitySource.StartActivity(activityName, ActivityKind.Consumer, parentContext.ActivityContext);

                activity!.SetActivityContext(eventName, "receive");

                try
                {
                    activity?.SetTag("message", message);

                    logger.LogInformation("Consuming a Kafka event: {@Topic} {@Message}", consumeResult.Topic, message);

                    if (message.Contains("Exception", StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new InvalidOperationException($"Event contains an exception: \"{message}\"");
                    }

                    if (await ProcessEvent(eventName, message))
                    {
                        consumer.StoreOffset(consumeResult);
                    }

                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Error processing message \"{Message}\"", message);

                    activity!.SetExceptionTags(ex);
                }
            }
        }


        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("Processing Kafka event: {EventName}", eventName);
            }

            await using var scope = serviceScopeFactory.CreateAsyncScope();

            if (!_subscriptionInfo.EventTypes.TryGetValue(eventName, out var eventType))
            {
                logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
                return false;
            }

            var integrationEvent = DeserializeMessage(message, eventType);

            var inboxService = scope.ServiceProvider.GetRequiredService<IInboxService>();

            // by default an event is consumed unless there is an error

            bool consumed = true;

            // Get all the handlers using the event type as the key
            foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
            {
                string handlerName = handler.GetType().FullName!;
                bool isConsumable = await inboxService.TryProcessAsync(integrationEvent!, handlerName);

                if (isConsumable)
                {
                    try
                    {
                        await handler.Handle(integrationEvent!);
                        await inboxService.SucceedAsync(integrationEvent!.Id, handlerName);
                    }
                    catch (Exception ex)
                    {

                        consumed = false;
                        logger.LogError(ex, "Failed to consume {@Event}", integrationEvent);
                        await inboxService.FailAsync(integrationEvent!.Id, handlerName);
                    }
                }
                else
                {
                    //do nothing, assume it was consumed
                }
            }

            return consumed;

        }

        private static IntegrationEvent? DeserializeMessage(string message, Type eventType)
        {
            return JsonSerializer.Deserialize(message, eventType) as IntegrationEvent;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            consumer.Unsubscribe();
            return Task.CompletedTask;
        }

        private static IEnumerable<string> ExtractTraceContextFromMessage(Message<string, string> message, string key)
        {
            if (message.Headers.TryGetLastBytes(key, out var bytes))
            {
                var value = Encoding.UTF8.GetString(bytes);
                return [value];
            }
            return [];
        }
    }

}
