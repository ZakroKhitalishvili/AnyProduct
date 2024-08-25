
using AnyProduct.Orders.Domain.Entities.Basket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnyProduct.Orders.Infrastructure.EntityConfigurations;

public class BasketItemEntityTypeConfiguration
    : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> baskeItemConfiguration)
    {
        baskeItemConfiguration.ToTable("basket_items");

        baskeItemConfiguration.HasKey(b => b.Id);

    }
}
