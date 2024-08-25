
using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using AnyProduct.OutBox.EF;
using AnyProduct.Products.Application;
using AnyProduct.Inbox.EF;

namespace AnyProduct.Products.Infrastructure;

public class ProductContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }

    private readonly IMediator _mediator;

    public ProductContext(DbContextOptions<ProductContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("products");
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryEntityTypeConfiguration());
        modelBuilder.UseOutboxEventLogs();
        modelBuilder.UseInboxEventLogs();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected async Task DispatchDomainEventsAsync()
    {
        var domainEntities = this.ChangeTracker
        .Entries<AggregateRoot>()
        .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearEvents());

        foreach (var domainEvent in domainEvents)
        {
            var notification = (INotification)Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
            await _mediator.Publish(notification);
        }
    }

}

#nullable enable
