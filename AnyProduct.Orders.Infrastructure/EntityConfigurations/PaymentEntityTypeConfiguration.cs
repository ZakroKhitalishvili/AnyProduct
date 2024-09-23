
using AnyProduct.Orders.Domain.Entities.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class PaymentEntityTypeConfiguration
    : IEntityTypeConfiguration<Payment>
{
    public void Configure([NotNull] EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(b => b.AggregateId);

    }
}
