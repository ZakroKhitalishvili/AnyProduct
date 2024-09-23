using AnyProduct.Orders.Domain.Entities.BasketAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class BasketEntityTypeConfiguration
    : IEntityTypeConfiguration<CustomerBasket>
{
    public void Configure([NotNull] EntityTypeBuilder<CustomerBasket> builder)
    {
        builder.ToTable("baskets");

        builder.Ignore(b => b.DomainEvents);

        builder.HasKey(b => b.AggregateId);

        builder.HasMany(b => b.Items).WithOne();

        builder.HasIndex(b => b.AggregateId)
            .IsUnique(true);
    }
}
