
using AnyProduct.Orders.Domain.Entities.Balance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class PaymentEntityTypeConfiguration
    : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> payemntConfiguration)
    {
        payemntConfiguration.ToTable("payments");

        payemntConfiguration.HasKey(b => b.AggregateId);

    }
}
