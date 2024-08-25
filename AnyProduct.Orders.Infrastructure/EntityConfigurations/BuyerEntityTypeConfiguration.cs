using AnyProduct.Orders.Domain.Entities.Buyer;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class BuyerEntityTypeConfiguration
    : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> buyerConfiguration)
    {
        buyerConfiguration.ToTable("buyers");

        buyerConfiguration.Ignore(b => b.DomainEvents);

        buyerConfiguration.HasKey(b => b.AggregateId);

        buyerConfiguration.HasMany(b => b.PaymentMethods).WithOne();

        buyerConfiguration.HasIndex(b => b.AggregateId)
            .IsUnique(true);

    }
}
