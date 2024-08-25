using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public interface IEventBusBuilder
{
    public IServiceCollection Services { get; }
}
