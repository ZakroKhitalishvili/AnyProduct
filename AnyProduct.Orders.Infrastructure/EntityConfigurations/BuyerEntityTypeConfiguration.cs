using AnyProduct.Orders.Domain.Entities.BuyerAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class BuyerEntityTypeConfiguration
    : IEntityTypeConfiguration<Buyer>
{
    public void Configure([NotNull] EntityTypeBuilder<Buyer> builder)
    {
        builder.ToTable("buyers");

        builder.Ignore(b => b.DomainEvents);

        builder.HasKey(b => b.AggregateId);

        builder.HasMany(b => b.PaymentMethods).WithOne();

        builder.HasIndex(b => b.AggregateId)
            .IsUnique(true);

    }
}
