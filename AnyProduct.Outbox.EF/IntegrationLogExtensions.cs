
using AnyProduct.OutBox.EF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AnyProduct.OutBox.EF;

public static class IntegrationLogExtensions
{
    public static void UseOutboxEventLogs([NotNull] this ModelBuilder builder)
    {
        builder.Entity<IntegrationEventLogEntry>(builder =>
        {
            builder.ToTable("IntegrationEventLog");

            builder.HasKey(e => e.EventId);
        });
    }

    public static void AddOutbox<TContext>([NotNull] this IHostApplicationBuilder builder, Assembly integrationEventAssembly) where TContext : DbContext
    {

        builder.Services.AddScoped<IOutboxService, EFOutboxService<TContext>>(options =>
        {
            return new EFOutboxService<TContext>(options.GetRequiredService<TContext>(), integrationEventAssembly);
        });
    }
}
