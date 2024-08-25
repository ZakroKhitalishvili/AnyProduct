using AnyProduct.Orders.Domain.Entities.Buyer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class PaymentMethodEntityTypeConfiguration
    : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> paymentMethodConfiguration)
    {
        paymentMethodConfiguration.ToTable("payment_methods");

        paymentMethodConfiguration.HasKey(b => b.Id);

        paymentMethodConfiguration.Property(b => b.Id).ValueGeneratedNever();

    }
}
