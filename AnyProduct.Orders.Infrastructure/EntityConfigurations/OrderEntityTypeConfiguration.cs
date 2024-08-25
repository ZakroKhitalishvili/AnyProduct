using AnyProduct.Orders.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class OrderEntityTypeConfiguration
    : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> orderConfiguration)
    {
        orderConfiguration.ToTable("orders");

        orderConfiguration.Ignore(b => b.DomainEvents);

        orderConfiguration.OwnsOne(b => b.Address);

        orderConfiguration.HasKey(b => b.AggregateId);

        orderConfiguration.HasMany(b => b.OrderItems).WithOne();

        orderConfiguration.HasIndex(b => b.AggregateId)
            .IsUnique(true);
    }
}
