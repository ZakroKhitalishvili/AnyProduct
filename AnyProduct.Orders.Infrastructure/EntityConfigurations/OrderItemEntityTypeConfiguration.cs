
using AnyProduct.Orders.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class OrderItemEntityTypeConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
    {
        orderItemConfiguration.ToTable("order_items");

        orderItemConfiguration.HasKey(b => b.Id);

    }
}
