using AnyProduct.Orders.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class OrderEntityTypeConfiguration
    : IEntityTypeConfiguration<Order>
{
    public void Configure([NotNull] EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.Ignore(b => b.DomainEvents);

        builder.OwnsOne(b => b.Address);

        builder.HasKey(b => b.AggregateId);

        builder.HasMany(b => b.OrderItems).WithOne();

        builder.HasIndex(b => b.AggregateId)
            .IsUnique(true);
    }
}
