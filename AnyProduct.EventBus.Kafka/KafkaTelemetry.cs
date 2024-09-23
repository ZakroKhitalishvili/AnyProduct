using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
namespace AnyProduct.EventBus.Kafka;

public class KafkaTelemetry
{
    public const string ActivitySourceName = "AnyProduct.EventBus.Kafka";
    public ActivitySource ActivitySource { get; } = new(ActivitySourceName);
    public TextMapPropagator Propagator { get; } = Propagators.DefaultTextMapPropagator;
}
