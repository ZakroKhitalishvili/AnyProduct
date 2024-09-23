using AnyProduct.Inbox.EF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Inbox.EF;

public static class InboxEventLogExtensions
{
    public static void UseInboxEventLogs([NotNull] this ModelBuilder builder)
    {
        builder.Entity<InboxEventLogEntry>(builder =>
        {
            builder.ToTable("InboxEventLogEntry");

            builder.HasKey(e => new { e.EventId, e.Handler });

            builder.Property(e => e.Version).IsRowVersion();
        });
    }

    public static void AddInbox<TContext>([NotNull] this IHostApplicationBuilder builder) where TContext : DbContext
    {
        builder.Services.Configure<InboxOptions>(builder.Configuration.GetSection(InboxOptions.InboxOptionsKey));
        builder.Services.AddScoped<IInboxService, EFInboxService<TContext>>();
    }
}
