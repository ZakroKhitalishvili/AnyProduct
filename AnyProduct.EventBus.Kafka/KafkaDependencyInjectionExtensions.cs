using AnyProduct.EventBus.Abstractions;
using AnyProduct.EventBus.Kafka;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Microsoft.Extensions.Hosting;

public static class KafkaDependencyInjectionExtensions
{

    private const string SectionName = "EventBus";

    public static IEventBusBuilder AddKafkaEventBus(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Options support
        builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection(SectionName));

        builder.Services.AddSingleton(sp =>
        {
            using (var scope = sp.CreateScope())
            {
                var options = scope.ServiceProvider.GetService<IOptions<EventBusOptions>>()!.Value;

                var kafkaConfig = new ProducerConfig
                {
                    BootstrapServers = options.BootstrapServers,
                    ClientId = options.ClientId,
                    AllowAutoCreateTopics = true,
                    EnableIdempotence = true,
                };
                return new ProducerBuilder<string, string>(kafkaConfig).Build();
            }
        });

        builder.Services.AddSingleton(sp =>
        {
            using (var scope = sp.CreateScope())
            {
                var options = scope.ServiceProvider.GetService<IOptions<EventBusOptions>>()!.Value;

                var kafkaConfig = new ConsumerConfig
                {
                    BootstrapServers = options.BootstrapServers,
                    ClientId = options.ClientId,
                    GroupId = options.GroupId,
                    AllowAutoCreateTopics = true,
                    EnableAutoOffsetStore = false,
                };
                return new ConsumerBuilder<string, string>(kafkaConfig).Build();
            }
        });

        builder.Services.AddSingleton(sp =>
        {
            using (var scope = sp.CreateScope())
            {
                var options = scope.ServiceProvider.GetService<IOptions<EventBusOptions>>()!.Value;

                var kafkaConfig = new AdminClientConfig
                {
                    BootstrapServers = options.BootstrapServers,
                    ClientId = options.ClientId,
                    AllowAutoCreateTopics = true,
                };

                return new AdminClientBuilder(kafkaConfig).Build();
            }
        });

        builder.Services.AddSingleton<IEventBus, KafkaEventBusProducer>();
        // Start consuming messages as soon as the application starts
        builder.Services.AddHostedService<KafkaEventBusConsumer>();

        return new EventBusBuilder(builder.Services);
    }

    private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
}
