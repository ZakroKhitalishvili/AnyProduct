
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using AnyProduct.OutBox.EF;
using AnyProduct.Orders.Domain.Entities.OrderAggregate;
using AnyProduct.Orders.Domain.Entities.BuyerAggregate;
using AnyProduct.Orders.Infrastructure.EntityConfigurations;
using AnyProduct.Orders.Domain.Entities;
using AnyProduct.Orders.Application;
using AnyProduct.Orders.Domain.Entities.BasketAggregate;
using AnyProduct.Inbox.EF;
using AnyProduct.Orders.Domain.Entities.PaymentAggregate;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Infrastructure;

public class OrderContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<Buyer> Buyers { get; set; }

    public DbSet<CustomerBasket> Baskets { get; set; }

    public DbSet<Payment> Payments { get; set; }


    private readonly IMediator _mediator;

    public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BasketEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BasketEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
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
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities
            .ForEach(entity => entity.Entity.ClearEvents());

        foreach (var domainEvent in domainEvents)
        {
            var notification = (INotification)Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
            await _mediator.Publish(notification);
        }
    }

}

