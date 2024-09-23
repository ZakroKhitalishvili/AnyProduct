using AnyProduct.Orders.Domain.Entities.BuyerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class PaymentMethodEntityTypeConfiguration
    : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure([NotNull] EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id).ValueGeneratedNever();

    }
}
