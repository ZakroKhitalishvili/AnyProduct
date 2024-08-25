using AnyProduct.Orders.Domain.Entities.Basket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class BasketEntityTypeConfiguration
    : IEntityTypeConfiguration<CustomerBasket>
{
    public void Configure(EntityTypeBuilder<CustomerBasket> basketConfiguration)
    {
        basketConfiguration.ToTable("baskets");

        basketConfiguration.Ignore(b => b.DomainEvents);

        basketConfiguration.HasKey(b => b.AggregateId);

        basketConfiguration.HasMany(b => b.Items).WithOne();

        basketConfiguration.HasIndex(b => b.AggregateId)
            .IsUnique(true);
    }
}
